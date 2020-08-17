using DecoderCore;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MapUpgrader
{
    public partial class MainWindow : Form
    {
        private static Assembly ExeAssembly = Assembly.GetExecutingAssembly();
        private static readonly string AppName = ExeAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
        private static readonly AssemblyCopyrightAttribute Copyright = ExeAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
        private static readonly Version Version = ExeAssembly.GetName().Version;
        private static readonly string VersionName = Version.Major + "." + Version.Minor + "." + Version.Build;

        private readonly OpenFileDialog fileChooser;
        private readonly FolderBrowserDialog folderBrowser;

        private FileInfo fileInfo;
        private DirectoryInfo directoryInfo;
        private FileInfo[] maps;

        private TreeNodeTag file;
        private TreeNodeTag groupsContainer; //EGRPVERS
        private TreeNodeTag entitiesContainer; //ENTLVERS
        private TreeNodeTag blueprintsContainer; //BPLTVERS

        private int next_trigger_id;
        private int res_group_id;
        private int blueprint_id_lab;
        private List<int> blueprint_ids_res = new List<int>();
        private List<int> ids_res = new List<int>();

        private enum UpgradeError { None, EntGrp, TrgResOn, TrgResOff}
        private UpgradeError lastError;

        public MainWindow()
        {
            InitializeComponent();

            Text = AppName + " (v" + VersionName + ")";

            toolTip.SetToolTip(BtnLoadMap, "Select a map file to add triggers to");
            toolTip.SetToolTip(BtnSelectFolder, "Select a folder to go through all of it's child maps");
            toolTip.SetToolTip(BtnTrash, "Clear log and unload map / folder");
            toolTip.SetToolTip(BtnUpgrade, "Append triggers to the selected map / to all maps under the selected folder");

            ClearLog();

            LogWriteLine("Initializing...");
            fileChooser = new OpenFileDialog
            {
                Title = "Choose a Map File",
                Filter = "Map File (*.sgb)|*.sgb",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            folderBrowser = new FolderBrowserDialog();

            BlankData();
            LogWriteLine("Ready.");
            LogWriteLine("");

            LogWriteLine("Load maps to append resources visibility triggers.");
            LogWriteLine("");
        }

        private void BlankData()
        {
            file = null;
            fileInfo = null;
            directoryInfo = null;
            maps = null;
            BtnUpgrade.Enabled = false;

            next_trigger_id = -1;
            res_group_id = -1;
            blueprint_id_lab = -1;
            blueprint_ids_res.Clear();
            ids_res.Clear();
        }

        private void ClearLog()
        {
            RichLogBox.Text = "";
            LogWriteLine(Copyright.Copyright);
            LogWriteLine("  - This tool is using the LuaInterface project: https://code.google.com/archive/p/luainterface/");
            LogWriteLine("   (under the MIT License: https://opensource.org/licenses/MIT)");
            LogWriteLine("");
        }

        private void LogWrite(string str)
        {
            RichLogBox.Text += str;
            RichLogBox.SelectionStart = RichLogBox.TextLength;
            RichLogBox.ScrollToCaret();
        }

        private void LogWriteLine(string str)
        {
            LogWrite(str + "\r\n");
        }

        private void LoadMapFile(string filePath, bool withLog)
        {
            fileInfo = new FileInfo(filePath);

            if (withLog) {
                LogWrite("Loading map: ");
                LogWriteLine(fileInfo.FullName);
            }

            file = null;
            try
            {
                file = new TreeNodeTag(fileInfo.Name, Decoder.ReadFile(fileInfo.FullName));
            }
            catch (Exception exception)
            {
                LogWriteLine("ERROR! " + exception.Message);
            }
        }

        private void BtnLoadMap_Click(object sender, EventArgs e)
        {
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                LoadMapFile(fileChooser.FileName, true);

                if (file == null)
                {
                    LogWriteLine("");
                    LogWriteLine("ERROR! Failed to load map file");
                    LogWrite("Unloading map: ");
                    LogWriteLine(fileInfo.Name);
                }
                else if (!CanUpgrade(true))
                {
                    LogWriteLine("");
                    LogWriteLine("ERROR! In order to upgrade a map the following items should no exist:");
                    LogWriteLine(" - Entity group named: resources");
                    LogWriteLine(" - Trigger named: resources_on");
                    LogWriteLine(" - Trigger named: resources_off");
                    LogWrite("Unloading map: ");
                    LogWriteLine(fileInfo.Name);
                }
                else
                {
                    LogWriteLine("Map is loaded, let's add some triggers!");
                    LogWriteLine("");

                    BtnUpgrade.Enabled = true;
                    return;
                }
            }
            else
            {
                LogWriteLine("No file was selected.");
            }

            LogWriteLine("");
            BlankData();
        }

        private void LoadMaps(string folderPath)
        {
            directoryInfo = new DirectoryInfo(folderPath);

            LogWrite("selected folder: ");
            LogWriteLine(directoryInfo.FullName);

            maps = directoryInfo.GetFiles("*.sgb");
            Array.Sort(maps, new MapComparer());
        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                LoadMaps(folderBrowser.SelectedPath);

                if (maps == null)
                {
                    LogWriteLine("");
                    LogWriteLine("ERROR! Failed to load maps from folder");
                    LogWrite("Unloading folder: ");
                    LogWriteLine(directoryInfo.Name);
                }
                else if (maps.Length == 0)
                {
                    LogWriteLine("");
                    LogWriteLine("No maps were found in the selected folder");
                    LogWrite("Unloading folder: ");
                    LogWriteLine(directoryInfo.Name);
                }
                else
                {
                    LogWriteLine("Folder is loaded with maps:");
                    LogWrite(" - ");
                    LogWriteLine(string.Join("\r\n - ", (object[])maps));
                    LogWriteLine("let's add some triggers!");
                    LogWriteLine("");

                    BtnUpgrade.Enabled = true;
                    return;
                }
            }

            LogWriteLine("");
            BlankData();
        }

        private void UpgradeMap(string filePath)
        {
            LogWriteLine("Inserting triggers data to:");
            LogWrite(" - ");
            LogWriteLine(filePath);

            LogWrite(" > Collecting data...");
            foreach (TreeNode node in blueprintsContainer.Nodes)
            {
                if (node.Text.Contains("FORM"))
                {
                    byte[] bprntName = ((TreeNodeTag)node.Nodes[2]).Tag.Data;
                    byte[] nameBytes = new byte[bprntName.Length - 17];
                    Array.Copy(bprntName, 17, nameBytes, 0, nameBytes.Length);
                    string name = System.Text.Encoding.ASCII.GetString(nameBytes);

                    if (name.Equals(@"DATA:ART\EBPS\STRUCTURES\LAB.EBP"))
                    {
                        byte[] bprntId = ((TreeNodeTag)node.Nodes[1]).Tag.Data;
                        byte[] idBytes = new byte[4];
                        Array.Copy(bprntId, 8, idBytes, 0, 4);
                        blueprint_id_lab = (int)DataParser.Parse(-1, DataParser.ParseType.Int32, idBytes);
                    }
                    else if (
                        name.Equals(@"DATA:ART\EBPS\NATURE\COAL_01.EBP") ||
                        name.Equals(@"DATA:ART\EBPS\NATURE\COAL_02.EBP") ||
                        name.Equals(@"DATA:ART\EBPS\NATURE\COAL_03.EBP") ||
                        name.Equals(@"DATA:ART\EBPS\NATURE\GEYSER.EBP") ||
                        name.Equals(@"DATA:ART\EBPS\NATURE\GEYSER_WATER.EBP"))
                    {
                        byte[] bprntId = ((TreeNodeTag)node.Nodes[1]).Tag.Data;
                        byte[] idBytes = new byte[4];
                        Array.Copy(bprntId, 8, idBytes, 0, 4);
                        blueprint_ids_res.Add((int)DataParser.Parse(-1, DataParser.ParseType.Int32, idBytes));
                    }
                }
            }

            foreach (TreeNode node in entitiesContainer.Nodes)
            {
                if (node.Text.Contains("FORM"))
                {
                    byte[] enttData = ((TreeNodeTag)node.Nodes[1]).Tag.Data;
                    byte[] bprntIdBytes = new byte[4];
                    Array.Copy(enttData, 12, bprntIdBytes, 0, 4);
                    int bprntId = (int)DataParser.Parse(-1, DataParser.ParseType.Int32, bprntIdBytes);

                    if (blueprint_ids_res.Contains(bprntId))
                    {
                        byte[] enttIdBytes = new byte[4];
                        Array.Copy(enttData, 8, enttIdBytes, 0, 4);
                        ids_res.Add((int)DataParser.Parse(-1, DataParser.ParseType.Int32, enttIdBytes));
                    }
                }
            }
            ids_res.Sort();
            LogWriteLine(" Done :)");

            byte[] group = new byte[0];
            string groupName = "resources";

            LogWrite(" > Building file...");
            group = MergeArrays(group, BitConverter.GetBytes(res_group_id));
            group = MergeArrays(group, BitConverter.GetBytes(groupName.Length));
            group = MergeArrays(group, System.Text.Encoding.ASCII.GetBytes(groupName));
            group = MergeArrays(group, BitConverter.GetBytes(ids_res.Count));
            foreach (int id in ids_res)
            {
                group = MergeArrays(group, BitConverter.GetBytes(id));
            }

            byte[] length = BitConverter.GetBytes(group.Length);
            Array.Reverse(length);
            group = MergeArrays(length, group);
            group = MergeArrays(System.Text.Encoding.ASCII.GetBytes("ESGP"), group);

            groupsContainer.AppendToSelf(group);
            LogWriteLine(" Done :)");

            LogWrite(" > Exporting file...");
            File.WriteAllBytes(filePath, file.Build());
            LogWriteLine(" Done :)");

            FileInfo triggersInfo = new FileInfo(Path.ChangeExtension(filePath, ".trg"));
            string triggers = TrgConsts.resourcesTriggers
                .Replace(TrgConsts.trigger1Id, 0.ToString())
                .Replace(TrgConsts.trigger2Id, 1.ToString())
                .Replace(TrgConsts.triggerLabId, blueprint_id_lab.ToString())
                .Replace(TrgConsts.triggerResGrpId, res_group_id.ToString());

            LogWrite(" > Exporting triggers...");
            if (triggersInfo.Exists)
            {
                List<string> lines = new List<string>(File.ReadAllLines(triggersInfo.FullName));
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("}"))
                    {
                        lines.Insert(i, "   \"" + TrgConsts.triggerFolder + "\",");
                        break;
                    }
                }
                lines.Add(triggers);
                File.WriteAllLines(triggersInfo.FullName, lines.ToArray());
            }
            else
            {
                string triggersPrefix = TrgConsts.triggersFoldersTable
                    .Replace(TrgConsts.triggersPath, triggersInfo.FullName);
                File.WriteAllText(triggersInfo.FullName, triggersPrefix + "\r\n" + triggers);
            }
            LogWriteLine(" Done :)");
            LogWrite("Upgrade complete for map: ");
            LogWriteLine(filePath);
            LogWriteLine("");
        }

        private void BtnUpgrade_Click(object sender, EventArgs e)
        {
            if (fileInfo != null)
            {
                UpgradeMap(fileInfo.FullName);
            }
            else if (maps != null)
            {
                foreach (FileInfo map in maps)
                {
                    LoadMapFile(map.FullName, false);
                    if (CanUpgrade(false))
                    {
                        UpgradeMap(map.FullName);
                    }
                    else
                    {
                        LogWrite("Map unupgradeable!");
                        switch (lastError)
                        {
                            case UpgradeError.EntGrp:
                                LogWriteLine(" Entity group named 'resources' already exists.");
                                break;
                            case UpgradeError.TrgResOn:
                                LogWriteLine(" Trigger named 'resources_on' already exists.");
                                break;
                            case UpgradeError.TrgResOff:
                                LogWriteLine(" Trigger named 'resources_off' already exists.");
                                break;
                            default:
                                LogWriteLine("");
                                break;
                        }
                        LogWrite(" - ");
                        LogWriteLine(map.FullName);
                        LogWriteLine("");
                    }
                }
                LogWrite("Upgrade complete for folder: ");
                LogWriteLine(directoryInfo.FullName);
                LogWriteLine("");
            }
        }

        private void BtnTrash_Click(object sender, EventArgs e)
        {
            BlankData();
            ClearLog();

            LogWriteLine("Load maps to append resources visibility triggers.");
            LogWriteLine("");
        }

        private void RichLogBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private bool CanUpgrade(bool withLog)
        {
            next_trigger_id = 0;

            if (withLog) LogWrite(" > Searching for an entity group...");
            groupsContainer = LookForContainerThatHasChild(file, "EGRPVERS");

            foreach (TreeNode node in groupsContainer.Nodes)
            {
                if (node is TreeNodeTag treeNodeTag)
                {
                    if (treeNodeTag.Tag.Name.Equals("ESGP"))
                    {
                        byte[] data = treeNodeTag.Tag.Data;

                        int length = (int)DataParser.Parse(treeNodeTag.Tag.Position, DataParser.ParseType.Int32, data.SubArray(4, 4));
                        string name = (string)DataParser.Parse(treeNodeTag.Tag.Position, DataParser.ParseType.String, data.SubArray(8, length));

                        if (name.ToLower().Equals("resources"))
                        {
                            if (withLog) LogWriteLine(" Group named 'resources' was found >:(");
                            lastError = UpgradeError.EntGrp;
                            return false;
                        }
                    }
                }
            }
            res_group_id = groupsContainer.Nodes.Count - 1;
            if (withLog) LogWriteLine(" Group named 'resources' was not found :)");

            if (withLog) LogWrite(" > Searching for triggers...");
            FileInfo triggersInfo = new FileInfo(Path.ChangeExtension(fileInfo.FullName, ".trg"));
            if (triggersInfo.Exists)
            {
                Console.Out.WriteLine("fileToLoad: " + triggersInfo.FullName);

                Lua lua = new Lua();
                lua.DoFile(triggersInfo.FullName);

                foreach (DictionaryEntry folder in lua.GetTable("folders"))
                {
                    LuaTable triggerFolder = lua.GetTable(folder.Value.ToString());
                    if (triggerFolder != null)
                    {
                        foreach (DictionaryEntry trigger in triggerFolder)
                        {
                            LuaTable triggerTable = (LuaTable)trigger.Value;
                            next_trigger_id = Math.Max(next_trigger_id, int.Parse(triggerTable["id"].ToString()) + 1);

                            string name = triggerTable["name"].ToString();
                            if ("resources_on".Equals(name))
                            {
                                if (withLog) LogWriteLine(" Trigger named 'resources_on' was found >:(");
                                lastError = UpgradeError.TrgResOn;
                                return false;
                            }
                            else if ("resources_off".Equals(name))
                            {
                                if (withLog) LogWriteLine(" Trigger named 'resources_off' was found >:(");
                                lastError = UpgradeError.TrgResOff;
                                return false;
                            }
                        }
                    }
                }
                if (withLog) LogWriteLine(" Triggers for resources control were not found :)");
            }
            else
            {
                if (withLog) LogWriteLine(" Triggers file was not found :)");
            }

            if (withLog) LogWriteLine(" > Loading map chunks...");
            entitiesContainer = LookForContainerThatHasChild(file, "ENTLVERS");
            blueprintsContainer = LookForContainerThatHasChild(file, "BPLTVERS");

            return true;
        }

        private TreeNodeTag LookForContainerThatHasChild(TreeNodeTag root, string childTagName)
        {
            foreach (TreeNode tag in root.Nodes)
            {
                if (tag is TreeNodeTag treeNodeTag)
                {
                    TreeNodeTag sub = null;
                    if (treeNodeTag.Tag.Name.Equals(childTagName))
                    {
                        return root;
                    }
                    else if (treeNodeTag.Tag.Name.Equals(Consts.Tags.FORM))
                    {
                        sub = LookForContainerThatHasChild(treeNodeTag, childTagName);
                    }

                    if (sub != null)
                    {
                        return sub;
                    }
                }
            }

            return null;
        }

        private byte[] MergeArrays(byte[] arr1, byte[] arr2)
        {
            byte[] newArr = new byte[arr1.Length + arr2.Length];
            Array.Copy(arr1, 0, newArr, 0, arr1.Length);
            Array.Copy(arr2, 0, newArr, arr1.Length, arr2.Length);
            return newArr;
        }

        private class MapComparer : IComparer<FileInfo>
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(FileInfo x, FileInfo y)
            {
                return x.FullName.CompareTo(y.FullName);
            }
        }
    }
}
