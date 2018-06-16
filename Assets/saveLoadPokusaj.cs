using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class saveLoadPokusaj : MonoBehaviour {

    
    public class SaveLoad
    {
        private string fileName;
        private Time gameTime;

        public SaveLoad(string fileName, Time gameTime)
        {
            this.fileName = fileName;
            this.gameTime = gameTime;
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
            }
        } 

        public Time GameTime
        {
            get
            {
                return this.gameTime;
            }
            set
            {
                this.gameTime = value;
            }
        } 
    }
    public class FileInformation
    {

        private static FileInformation fileInformation;

        private Dictionary<string, SaveLoad>  saveLoadDictionary;
        private BinaryFormatter formatter;

        private const string DATA_FILENAME = "fileInformat.dat";

        public static FileInformation Instance()
        {
            if (fileInformation == null)
            {
                fileInformation = new FileInformation();
            } 

            return fileInformation;
        } 

        private FileInformation ()
        {
            
            this.saveLoadDictionary = new Dictionary<string, SaveLoad>();
            this.formatter = new BinaryFormatter();
        } 

        public void SaveLoad(string name,Time gameTime)
        {
            
            if (this.saveLoadDictionary.ContainsKey(name))
            {
                Console.WriteLine("You had already added " + name + " before.");
            }
            
            else
            {
               
                this.saveLoadDictionary.Add(name, new SaveLoad(name, gameTime));
                Console.WriteLine(" GAME SAVED.");
            } 
        } 

        public void RemoveSave(string name)
        {
           
            if (!this.saveLoadDictionary.ContainsKey(name))
            {
                Console.WriteLine(name + " had not been added before.");
            }
            
            else
            {
                if (this.saveLoadDictionary.Remove(name))
                {
                    Console.WriteLine(name + " had been removed successfully.");
                }
                else
                {
                    Console.WriteLine("Unable to remove " + name);
                } 
            } 
        } 

        public void Save()
        {
            
            try
            {
                
                FileStream writerFileStream =
                    new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
               
                this.formatter.Serialize(writerFileStream, this.saveLoadDictionary);

                .
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to save our friends' information");
            } 
        } 

        public void Load()
        {

            if (File.Exists(DATA_FILENAME))
            {

                try
                {
                    
                    FileStream readerFileStream = new FileStream(DATA_FILENAME,
                        FileMode.Open, FileAccess.Read);
                    
                    this.saveLoadDictionary = (Dictionary<String, SaveLoad>)
                        this.formatter.Deserialize(readerFileStream);
                  
                    readerFileStream.Close();

                }
                catch (Exception)
                {
                    Console.WriteLine("There seems to be a file that contains " +
                        "Save information but somehow there is a problem " +
                        "with reading it.");
                } 

            } 

        } 

        public void Print()
        {
            
            if (this.saveLoadDictionary.Count > 0)
            {
                Console.WriteLine("Name, Email");
                foreach (SaveLoad save in this.saveLoadDictionary.Values)
                {
                    Console.WriteLine(save.FileName + ", " + save.GameTime);
                } 
            }
            else
            {
                Console.WriteLine("There are no saved games");
            } 
        } 

    } 




}
	
