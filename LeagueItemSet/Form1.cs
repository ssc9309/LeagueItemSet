using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;



/*notes
 * [05:42:40] -         <div id="Lane" style="margin-top:-50px; position: absolute;"></div>
[05:42:40] -         <div id="content_lane"></div>
[05:42:40] -         <div id="Jungle" style="margin-top:-50px; position: absolute;"></div>
[05:42:40] -         <div id="content_jungle"></div>
[05:42:40] -         <div id="Support" style="margin-top:-50px; position: absolute;"></div>
[05:42:40] -         <div id="content_support"></div>
[05:42:40] -         <div id="ARAM" style="margin-top:-50px; position: absolute;"></div>
[05:42:40] -         <div id="content_aram"></div>
 * 
 * 
 * 
 * 
 * 
 * <td style="text-align:left;" data-sort-value="Aatrox" bgcolor="#242424"><span class="character_icon" style="white-space: nowrap"><a href="/wiki/Aatrox" class="image image-thumbnail link-internal" title="Aatrox"><img src="http://img4.wikia.nocookie.net/__cb20150402215200/leagueoflegends/images/thumb/c/cc/AatroxSquare.png/20px-AatroxSquare.png" alt="AatroxSquare.png" class="" data-image-key="AatroxSquare.png" data-image-name="AatroxSquare.png" width="20" height="20"></a>&nbsp;<span><a href="/wiki/Aatrox" title="Aatrox">Aatrox</a></span></span>
</td>
 */

namespace LeagueItemSet
{
    public partial class Form1 : Form
    {
        //private string websiteURL = "http://www.lolflavor.com/";
        //private string websiteURL = "http://www.lolflavor.com/champions/Aatrox/Recommended/Aatrox_lane_scrape.json";
        //private string websiteURL = "http://www.lolflavor.com/champions/asdfasdf/";
        //private string championListURL = "http://leagueoflegends.wikia.com/wiki/List_of_champions";
        
        //private string allChampionsString = "<h4> All Champions</h4>";
        //private string championLinkString = "<a class=\"champion text-center btn btn-primary\"";
        private List<string> championList = new List<string>();
        private List<string> itemSetList = new List<string>();


        public Form1()
        {
            InitializeComponent();

            UpdateProgressBox("I am assuming that you installed league to C drive");
        }

        public void UpdateProgressBox(string line)
        {
            string timeStamp = DateTime.Now.ToString("[hh:mm:ss]");

            line += "\n";

            //InvokeRequired is needed for accessing this element in a different thread.
            if (rtbx_ProgressBox.InvokeRequired)
            {
                rtbx_ProgressBox.Invoke((Action)(() => rtbx_ProgressBox.AppendText(timeStamp + " - ")));
                rtbx_ProgressBox.Invoke((Action)(() => rtbx_ProgressBox.AppendText(line)));
                rtbx_ProgressBox.Invoke((Action)(() => rtbx_ProgressBox.SelectionStart = rtbx_ProgressBox.Text.Length));
                rtbx_ProgressBox.Invoke((Action)(() => rtbx_ProgressBox.ScrollToCaret()));
            }
            else
            {
                rtbx_ProgressBox.AppendText(timeStamp + " - ");
                rtbx_ProgressBox.AppendText(line);
                rtbx_ProgressBox.SelectionStart = rtbx_ProgressBox.Text.Length;
                rtbx_ProgressBox.ScrollToCaret();
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            championList.Clear();
            itemSetList.Clear();

            btn_Update.Enabled = false;

            Thread LoadItemSetsThread = new Thread(new ThreadStart(LoadItemSets));
            LoadItemSetsThread.IsBackground = true;
            LoadItemSetsThread.Start();
        }

        private void LoadItemSets()
        {
            PopulateChampionList();
            PopulateItemSetList();
            DownloadItemSets();


            /*
            try
            {
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = GetResponse(websiteURL);

                string line = readStream.ReadLine();
                bool championListStart = false;

                while (line != null)
                {
                    UpdateProgressBox(line);

                    if (line.IndexOf(allChampionsString) >= 0)
                    {
                        //UpdateProgressBox(line);
                        championListStart = true;
                    }

                    if (championListStart)
                    {
                        //<a class="champion text-center btn btn-primary" title="Aatrox" href="/champions/Aatrox">

                        if (line.IndexOf(championLinkString) >= 0)
                        {
                            string titleString = "title=\"";
                            line = line.Trim();
                            line = line.Substring(line.IndexOf(titleString) + titleString.Length);
                            line = line.Substring(0, line.IndexOf("\""));

                            string championName = line;
                        }

                    }


                    line = readStream.ReadLine();


                }
            }
            catch(Exception exc)
            {
                UpdateProgressBox("Shit happened. Errorrrrr: " + exc);
            }
             */
        }

        private void PopulateChampionList()
        {
            UpdateProgressBox("Populating champion list...");

            string championListURL = "http://leagueoflegends.wikia.com/wiki/List_of_champions";

            StreamReader sr =  GetResponse(championListURL);
            string line = sr.ReadLine();

            bool imDone = false;
            bool inChampTable = false;
            string champToken = "<td style=\"text-align:left;\" data-sort-value=\"";

            //<td style="text-align:left;" data-sort-value="Aatrox" bgcolor="#242424"><span class="character_icon" style="white-space: nowrap"><a href="/wiki/Aatrox" class="image image-thumbnail link-internal" title="Aatrox"><img src="http://img4.wikia.nocookie.net/__cb20150402215200/leagueoflegends/images/thumb/c/cc/AatroxSquare.png/20px-AatroxSquare.png" alt="AatroxSquare.png" class="" data-image-key="AatroxSquare.png" data-image-name="AatroxSquare.png" width="20" height="20"></a>&nbsp;<span><a href="/wiki/Aatrox" title="Aatrox">Aatrox</a></span></span></td>

            while (line != null && !imDone)
            {
                if (line.IndexOf(champToken) >= 0)
                {
                    inChampTable = true;

                    line = line.Trim();
                    line = line.Substring(champToken.Length);
                    line = line.Substring(0, line.IndexOf("\""));

                    //Replace any special characters.
                    //lolFOTM site does not like them
                    line = line.Replace(" ", "");
                    line = line.Replace("&#39;", "");
                    line = line.Replace(".", "");

                    //special fucking case
                    //for some fucking reason, lolflavor named Wukong => MonkeyKing
                    if (line == "Wukong")
                    {
                        line = "MonkeyKing";
                    }

                    championList.Add(line);
                }
                else
                {
                    //<table class="stdt sortable jquery-tablesorter" style="margin: 0 auto; text-align:Center; border-collapse:collapse; border:1px solid #191919; width:95%; color:white;" border="1" cellpadding="1" cellspacing="0">
                    if (line.IndexOf("</table>") >= 0 && inChampTable)
                    {
                        imDone = true;
                    }
                }

                line = sr.ReadLine();
            }

            /*
            foreach(string champ in championList)
            {
                UpdateProgressBox(champ);
            }
            */

            UpdateProgressBox("There are " + championList.Count + " champions on lol wiki");
        }

        private void PopulateItemSetList()
        {
            UpdateProgressBox("Populating item set list...");

            string lolFOTMPage = "http://www.lolflavor.com/";
            string lolFOTMChampPage = lolFOTMPage + "champions/";

            foreach(string champ in championList)
            {
                using (StreamReader sr = GetResponse(lolFOTMChampPage+champ))
                {
                    try
                    {
                        /*
                        <div id="content_lane"></div>
    [05:42:40] -         <div id="Jungle" style="margin-top:-50px; position: absolute;"></div>
    [05:42:40] -         <div id="content_jungle"></div>
    [05:42:40] -         <div id="Support" style="margin-top:-50px; position: absolute;"></div>
    [05:42:40] -         <div id="content_support"></div>
    [05:42:40] -         <div id="ARAM" style="margin-top:-50px; position: absolute;"></div>
    [05:42:40] -         <div id="content_aram"></div>
                         */

                        string line = sr.ReadLine();
                        string foo = "<div id=\"content_";


                        while (line != null)
                        {
                            if (line.IndexOf(foo) >= 0)
                            {
                                line = line.Trim();
                                line = line.Substring(foo.Length);
                                line = line.Substring(0, line.IndexOf("\""));

                                //there are some exceptions
                                //there are some <div id="content_somethingIDon'tWant">
                                if (line != "skills")
                                {
                                    //http://www.lolflavor.com/champions/Aatrox/Recommended/Aatrox_lane_scrape.json
                                    string itemSetJSONAddress = lolFOTMChampPage + champ + "/recommended/" + champ + "_" + line + "_scrape.json";
                                    itemSetList.Add(itemSetJSONAddress);
                                }
                            }

                            line = sr.ReadLine();
                        }
                    }
                    catch(Exception exc)
                    {
                        UpdateProgressBox("Error when trying to populate item list: " + exc);
                    }
                }
            }

            UpdateProgressBox(itemSetList.Count + " item sets have been found");
        }

        private void DownloadItemSets()
        {
            //C:\Riot Games\League of Legends\Config\Champions\Kalista\Recommended\

            string destFolderPath = "C:\\Riot Games\\League of Legends\\Config\\Champions\\<Champ>\\Recommended\\";

            //http://www.lolflavor.com/champions/Aatrox/Recommended/Aatrox_lane_scrape.json

            string lastChampName = "";
            bool isChampNameCorrect = false;

            foreach (string itemsetJSON in itemSetList)
            {
                try
                {
                    string champName = itemsetJSON.Substring("http://www.lolflavor.com/champions/".Length);
                    champName = champName.Substring(0, champName.IndexOf("/"));
                    string fileName = itemsetJSON.Substring(("http://www.lolflavor.com/champions/" + champName + "/Recommended/").Length);
                    fileName = "HS_" + fileName;

                    UpdateProgressBox("Writing file: " + fileName);

                    if (champName != lastChampName)
                    {
                        if (!isChampNameCorrect && lastChampName != "")
                        {
                            UpdateProgressBox("Last champ was not found");
                        }

                        lastChampName = champName;
                        isChampNameCorrect = false;
                    }


                    StreamReader sr = GetResponse(itemsetJSON);
                    string line = sr.ReadLine();

                    if (!Directory.Exists(destFolderPath.Replace("<Champ>", champName)))
                    {
                        Directory.CreateDirectory(destFolderPath.Replace("<Champ>", champName));
                    }

                    StreamWriter sw = new StreamWriter(destFolderPath.Replace("<Champ>", champName) + fileName);

                    sw.Write(line);

                    sw.Close();

                    isChampNameCorrect = true;
                }
                catch(Exception exc)
                {
                    UpdateProgressBox("something went wrong. so over it: " + exc);
                }
            }

        }


        StreamReader GetResponse(string url)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                return readStream;
            }
            catch(Exception exc)
            {
                UpdateProgressBox("Shit when wrong with http. Error: " + exc);
                return null;
            }
        }
    }
}
