using System;
using System.Collections.Generic;
using System.IO;

namespace copyDirAndFiles
{
    public class CopyToLowerCase
    {
        static public void Main (string[] args)
        {
            bool success = false;
            foreach (string arg in args) {
                Console.WriteLine("Args: " +arg);
            }
            
            if (args.Length == 1) {
                RenameFiles rename = new RenameFiles(args[(args.Length-1)]);
                success = rename.Rename();   
                Console.WriteLine("Rename() returned {0}", success);
            } else if (args.Length == 2) {
                string sourceDirectory = args[0];
                string targetDirectory = args[1];

                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetDirectory.ToLower());

                CopyDir.CopyAll(diSource, diTarget);

            }
            else
                Console.WriteLine("Illegal arguments.");

        }

        class CopyDir
        {
            public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
            {
                if (source.FullName.ToLower() == target.FullName.ToLower())
                {
                    return;
                }

                // Check if the target directory exists, if not, create it. 
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }

                // Copy each file into it's new directory. 
                foreach (FileInfo fi in source.GetFiles())
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }

                // Copy each subdirectory using recursion. 
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name.ToLower());
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
        }

           

        class RenameFiles {
            string path;

            public RenameFiles(string path) {
                this.path = path;
            }

            public bool Rename() {
                bool status = true;
                int nmbrOfFiles, nmbrOfDirs = 0;

                if(File.Exists(path)) 
                {
                    // This path is a file
                    ProcessFile(path); 
                }               
                else if(Directory.Exists(path)) 
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else 
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                    status = false;
                } 
                return status;
            }

            // Process all files in the directory passed in, recurse on any directories  
            // that are found, and process the files they contain. 
            public static void ProcessDirectory(string targetDirectory) 
            {             
                Console.WriteLine("targetDirectory: {0}", Path.GetFileName(targetDirectory).ToLower());
                    
                // Process the list of files found in the directory. 
                string [] fileEntries = Directory.GetFiles(targetDirectory);
                foreach(string fileName in fileEntries)
                    ProcessFile(fileName);

                // Recurse into subdirectories of this directory. 
                string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach(string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory.ToLower());
            }

            // Insert logic for processing found files here. 
            public static void ProcessFile(string path) 
            {
                
                string file = Path.GetFileName(path).ToLower();                
                System.IO.File.Move(path, (Path.GetDirectoryName(path)+Path.AltDirectorySeparatorChar+file));
                Console.WriteLine("Processed file '{0}' to '{1}'.", path, file);	    
            }
        }
    }
}
    