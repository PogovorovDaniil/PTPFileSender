﻿using GPeerToPeer.Client;
using PTPFileSender.Constants;
using PTPFileSender.Controllers;
using PTPFileSender.Models;
using System;
using System.IO;
using System.Threading;

namespace PTPFileSender.Services
{
    internal static class LoadFileService
    {
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static bool IsProcess { get; private set; } = false;
        public static void StopProcess()
        {
            tokenSource.Cancel();
        }
        public static (bool, ProcessResult) Cancel(PTPNode node, ProcessResult processResult = ProcessResult.Canceled)
        {
            if (PeerToPeerService.Get(out CancelRequest _, node))
            {
                tokenSource = new CancellationTokenSource();
                IsProcess = false;
                return (true, processResult);
            }
            if (tokenSource.Token.IsCancellationRequested)
            {
                tokenSource = new CancellationTokenSource();
                IsProcess = false;
                PeerToPeerService.Send(new CancelRequest() { IsCancel = true }, node);
                return (true, processResult);
            }
            return (false, ProcessResult.OK);
        }
        public static ProcessResult UploadProcess(string path, PTPNode node, IWindowEvents.MoveProgressBarHandler moveProgressBar)
        {
            if (IsProcess) return ProcessResult.Locked;
            while (PeerToPeerService.Get(out CancelRequest _, node)) ;
            IsProcess = true;
            FileInfo fileInfo = new FileInfo(path);
            using (FileStream fs = fileInfo.OpenRead())
            {
                FileInformation fileInformation = new FileInformation()
                {
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length
                };
                PeerToPeerService.Send(fileInformation, node);
                DownloadRequest request;
                while (!PeerToPeerService.Get(out request, node))
                {
                    (bool isCancel, ProcessResult processResult) = Cancel(node);
                    if (isCancel) return processResult;
                }
                if (request.IsDownload)
                {
                    while (true)
                    {
                        while(PeerToPeerService.GetFast(out Pieces pieces, node))
                        {
                            moveProgressBar?.Invoke(pieces.Progress);
                            foreach(int location in pieces.PieceIndexes)
                            {
                                byte[] buffer = new byte[FilePiece.PIECE_SIZE];
                                fs.Seek((long)location * FilePiece.PIECE_SIZE, SeekOrigin.Begin);
                                int size = fs.Read(buffer, 0, buffer.Length);
                                Array.Resize(ref buffer, size);
                                FilePiece filePiece = new FilePiece()
                                {
                                    Location = location,
                                    Piece = buffer
                                };
                                PeerToPeerService.SendFast(filePiece, node);
                            }
                        }
                        if(PeerToPeerService.Get(out EndRequest endRequest, node))
                        {
                            if (endRequest.IsEnd) break;
                        }
                        (bool isCancel, ProcessResult processResult) = Cancel(node);
                        if (isCancel) return processResult;
                    }
                }
            }
            IsProcess = false;
            return ProcessResult.OK;
        }
        public static ProcessResult DownloadProcess(FileInformation fileInformation, string path, PTPNode node, IWindowEvents.MoveProgressBarHandler moveProgressBar)
        {
            if (IsProcess) return ProcessResult.Locked;
            while (PeerToPeerService.Get(out CancelRequest _, node)) ;
            IsProcess = true;
            if(!PeerToPeerService.Send(new DownloadRequest() { IsDownload = true }, node))
            {
                IsProcess = false;
                return ProcessResult.Lost;
            }

            double progress = 0;

            bool[] received;
            int[] receivedIndexes;
            string filePath = path;
            using (FileStream fs = File.Create(filePath))
            {
                fs.SetLength(fileInformation.FileSize);
                received = new bool[(int)Math.Ceiling((double)fileInformation.FileSize / FilePiece.PIECE_SIZE)];
                receivedIndexes = new int[received.Length];
                for(int i = 0; i < received.Length; i++)
                {
                    received[i] = false;
                    receivedIndexes[i] = i;
                }
                
                int receivedIndexesEnd = 0;
                int index = 0;
                do
                {
                    if (index >= receivedIndexes.Length)
                    {
                        index = 0;
                        Array.Resize(ref receivedIndexes, receivedIndexesEnd);
                        receivedIndexesEnd = 0;
                    }
                    int[] piecesIndexes = new int[Pieces.PIECE_COUNT];
                    int j;
                    for (j = 0; j < piecesIndexes.Length; )
                    {
                        if (index >= receivedIndexes.Length) break;
                        if (!received[receivedIndexes[index]])
                        {
                            receivedIndexes[receivedIndexesEnd] = receivedIndexes[index];
                            piecesIndexes[j++] = receivedIndexes[index];
                            receivedIndexesEnd++;
                        }
                        index++;
                    }
                    Array.Resize(ref piecesIndexes, j);
                    Pieces pieces = new Pieces() { PieceIndexes = piecesIndexes, Progress = progress * 100 / received.Length };
                    PeerToPeerService.SendFast(pieces, node);
                    while (PeerToPeerService.GetFast(out FilePiece piece, node) && received[piece.Location] == false)
                    {
                        progress++;
                        fs.Seek((long)piece.Location * FilePiece.PIECE_SIZE, SeekOrigin.Begin);
                        foreach(var fileByte in piece.Piece) fs.WriteByte(fileByte);
                        received[piece.Location] = true;
                    }
                    (bool isCancel, ProcessResult processResult) = Cancel(node);
                    if (isCancel) return processResult;
                    moveProgressBar?.Invoke(progress * 100 / received.Length);
                } while (receivedIndexesEnd > 0) ;
                if(!PeerToPeerService.Send(new EndRequest() { IsEnd = true }, node))
                {
                    (bool isCancel, ProcessResult processResult) = Cancel(node, ProcessResult.Lost);
                    if (isCancel) return processResult;
                }
            }
            IsProcess = false;
            return ProcessResult.OK;
        }
    }
}
