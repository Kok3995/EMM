using MTPExplorer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MTPCon
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var collection = new PortableDeviceCollection();
            bool another;
            int? depth = null;
            bool folderOnly = false;
            collection.Refresh();
            var device = collection.First();
            device.Connect();
            do
            {
                //Console.Write("Enter depth: ");

                //if (int.TryParse(Console.ReadLine(), out int depthInt))
                //    depth = depthInt;

                //Console.WriteLine();
                //Console.Write("Folder Only? (y/n): ");
                //if (Console.ReadLine().ToLower().Equals("y"))
                //    folderOnly = true;

                //collection.Refresh();

                //foreach (var device in collection)
                //{
                //    device.Connect();
                //    Console.WriteLine("-------------------------\n" + device.FriendlyName + "\n-------------------------");
                //    DisplayFolderContents(device.GetContents(depth, folderOnly));
                //    device.DisConnect();
                //}
                //device.TransferToDevice(@"F:\LDPlayer\Garulea Continent.record", "o2");
                //var content = device.GetContentByPath(@"mtp:\HM NOTE 1LTEW\Internal storage\Android\Garulea Continent.record");
                var content = device.GetContentByPath(@"mtp:\HM NOTE 1LTEW\Internal storage\Android\A.txt");
                device.DeleteFile(content.Id, true);
                content = device.GetContentByPath(@"mtp:\HM NOTE 1LTEW\Internal storage\Android\Q.txt");
                device.DeleteFile(content.Id, true);

                Console.Write("Another? (y/n) ");
                if (!Console.ReadLine().ToLower().Equals("n"))
                    another = true;
                else
                    another = false;
            } while (another);
            device.DisConnect();
            //var device2 = collection.GetDeviceByName("HM NOTE 1LTEW");

            //device2.Connect();

            ////Console.WriteLine("Enter folder name and folder parent id");

            ////var folder = device2.NewFolder(Console.ReadLine(), Console.ReadLine());

            //var folder = device2.GetContentByPath(@"mtp:\HM NOTE 1LTEW\Internal storage\Android");

            //Console.WriteLine(folder.Id + " ||| " + folder.Name);
            //device2.DisConnect();

            //manager.CreateDirectory(@"mtp:\HM NOTE 1LTEW\Internal storage\AnkuLua\Test\Test 2");

            //Console.WriteLine(manager.Exist(@"mtp:\HM NOTE 1LTEW\Internal storage\AnkuLua\Test\Test 2").Id);

            //manager.Delete(@"mtp:\HM NOTE 1LTEW\Internal storage\AnkuLua\Test", true);

            //Console.WriteLine(manager.Exist(@"mtp:\HM NOTE 1LTEW\Internal storage\AnkuLua\Test\Test 2")?.Id ?? "NotFound");

            Console.ReadKey(true);
        }

        private static string step = "";

        public static void DisplayObject (PortableDeviceObject folder)
        {
            Console.WriteLine(step + folder.Id + " ||| " + folder.Name);
        }

        public static void DisplayFolderContents(PortableDeviceFolder folder)
        {
            step = "";
            int size;
            var queue = new Queue<PortableDeviceObject>();

            DisplayObject(folder);

            foreach (var item in folder.Files)
            {
                queue.Enqueue(item);
            }

            size = queue.Count;

            while (queue.Count > 0)
            {
                var currentItem = queue.Dequeue();

                DisplayObject(currentItem);

                if (currentItem is PortableDeviceFolder nextFolder)
                {
                    foreach (var item in nextFolder.Files)
                    {
                        queue.Enqueue(item);
                    }
                }

                if (--size < 1)
                {
                    size = queue.Count;
                    step += "-";
                }
            }
        }
    }
}
