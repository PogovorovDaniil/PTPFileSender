using GPeerToPeer.Client;
using PTPFileSender.Controllers;
using PTPFileSender.Helpers;
using PTPFileSender.Models;
using System;
using System.IO;

namespace PTPFileSender.Services
{
    internal static class LoadFileService
    {
        public static void UploadProcess(string path, PTPNode node, IWindowEvents.MoveProgressBarHandler moveProgressBar)
        {
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
                while (!PeerToPeerService.Get(out request, node)) ;
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
                            break;
                        }
                    }
                }
            }
        }
        public static void DownloadProcess(FileInformation fileInformation, string path, bool isDownload, PTPNode node, IWindowEvents.MoveProgressBarHandler moveProgressBar)
        {
            PeerToPeerService.Send(new DownloadRequest() { IsDownload = isDownload }, node);
            if (!isDownload) return;

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
                    if (index == receivedIndexes.Length)
                    {
                        index = 0;
                        Array.Resize(ref receivedIndexes, receivedIndexesEnd);
                        receivedIndexesEnd = 0;
                    }
                    int[] piecesIndexes = new int[Pieces.PIECE_COUNT];
                    int j;
                    for (j = 0; j < piecesIndexes.Length; j++)
                    {
                        if (index == receivedIndexes.Length) break;
                        if (!received[receivedIndexes[index]])
                        {
                            receivedIndexes[receivedIndexesEnd] = receivedIndexes[index];
                            piecesIndexes[j] = receivedIndexes[index];
                            receivedIndexesEnd++;
                        }
                        index++;
                    }
                    Array.Resize(ref piecesIndexes, j);
                    Pieces pieces = new Pieces() { PieceIndexes = piecesIndexes, Progress = progress * 100 / received.Length };
                    PeerToPeerService.SendFast(pieces, node);
                    while (PeerToPeerService.GetFast(out FilePiece piece, node))
                    {
                        progress++;
                        fs.Seek((long)piece.Location * FilePiece.PIECE_SIZE, SeekOrigin.Begin);
                        foreach(var fileByte in piece.Piece) fs.WriteByte(fileByte);
                        for(int i = 0; i < FilePiece.PIECE_SIZE; i++)
                        {
                            int pos = piece.Location * FilePiece.PIECE_SIZE + i;
                            if (pos > received.Length) break;
                            received[pos] = true;
                        }
                    }
                    moveProgressBar?.Invoke(progress * 100 / received.Length);
                } while (receivedIndexesEnd > 0) ;
                PeerToPeerService.Send(new EndRequest() { IsEnd = true }, node);
            }
        }
    }
}
