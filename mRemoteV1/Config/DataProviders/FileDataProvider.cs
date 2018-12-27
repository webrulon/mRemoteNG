﻿using System;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.DataProviders
{
    public class FileDataProvider : IDataProvider<string>
    {
        public string FilePath { get; set; }

        public FileDataProvider(string filePath)
        {
            FilePath = filePath;
        }

        public virtual string Load()
        {
            var fileContents = "";
            try
            {
                if (!File.Exists(FilePath))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Could not load file. File does not exist '{FilePath}'");
                    return "";
                }

                fileContents = File.ReadAllText(FilePath);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to load file {FilePath}", ex);
            }

            return fileContents;
        }

        public virtual void Save(string content)
        {
            try
            {
                CreateMissingDirectories();
                File.WriteAllText(FilePath, content);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to save file {FilePath}", ex);
            }
        }

        public virtual void MoveTo(string newPath)
        {
            try
            {
                File.Move(FilePath, newPath);
                FilePath = newPath;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to move file {FilePath} to {newPath}", ex);
            }
        }

        private void CreateMissingDirectories()
        {
            var dirname = Path.GetDirectoryName(FilePath);
            if (dirname == null) return;
            Directory.CreateDirectory(dirname);
        }
    }
}