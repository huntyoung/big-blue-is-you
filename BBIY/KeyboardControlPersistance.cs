using Microsoft.Xna.Framework.Input;
//
// Added to support serialization
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Diagnostics;
using System;

namespace BBIY
{
    public class KeyboardControlPersistance
    {
        private bool saving;
        private bool loading;

        public static KeyboardControls m_loadedControls;

        public KeyboardControlPersistance() 
        {
            m_loadedControls = null;
            saving = false;
            loading = false;
        }

        public bool isSaving()
        {
            return saving;
        }

        public bool isLoading()
        {
            return loading;
        }

        public void saveControls(Keys up, Keys down, Keys left, Keys right, Keys reset)
        {
            lock (this)
            {
                if (!this.saving)
                {
                    this.saving = true;
                    KeyboardControls keyboardControls = new KeyboardControls(up, down, left, right, reset);
                    finalizeSaveAsync(keyboardControls);
                }
            }
        }

        private async void finalizeSaveAsync(KeyboardControls state)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        storage.DeleteFile("KeyboardControls.xml");
                        using (IsolatedStorageFileStream fs = storage.OpenFile("KeyboardControls.xml", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(KeyboardControls));
                                mySerializer.Serialize(fs, state);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        Debug.WriteLine("Couldn't save controls");
                    }
                }

                this.saving = false;
            });
        }

        /// <summary>
        /// Demonstrates how to deserialize an object from storage device
        /// </summary>
        public void loadControls()
        {
            lock (this)
            {
                if (!this.loading)
                {
                    this.loading = true;
                    finalizeLoadAsync();
                }
            }
        }

        private async void finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        //foreach (var f in storage.GetFileNames())
                        //{
                        //    storage.DeleteFile(f);
                        //}
                        if (storage.FileExists("KeyboardControls.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("KeyboardControls.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(KeyboardControls));
                                    m_loadedControls = (KeyboardControls)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                        else
                        {
                            m_loadedControls = null;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("An Error Occurred, Couldn't Load Controls");
                    }
                }

                this.loading = false;
            });
        }
    }
}
