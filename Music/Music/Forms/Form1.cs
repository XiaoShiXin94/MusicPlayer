using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using Sunny.UI;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using AxWMPLib;
using System.Security.Policy;
using System.Net.Security;
using System.Text.RegularExpressions;
using Shell32;
using System.Web;
using DirectShowLib;
using System.Runtime.InteropServices;
using DirectShowLib.DES;

namespace Music.Forms
{
    public partial class Form1 : UIForm
    {
        //声明一个list，用来存储文件的路径
        List<string> urlList = new List<string>();//泛型
        double max, min;
        private Color Red;
        Songs song = new Songs();

        public Form1()
        {
            InitializeComponent();

            uiAvatar2.Location = new System.Drawing.Point(400, 50);
            uiAvatar2.Shape = UIShape.Circle;
            uiAvatar2.AvatarSize = 400;
            uiAvatar2.BackColor = UIColor.White;
            uiAvatar2.Width = uiAvatar2.AvatarSize;
            uiAvatar2.Height = uiAvatar2.AvatarSize;
            uiPanel3.Controls.Add(uiAvatar2);

            trackBar1.Value = 0;
            listBoxMusics.Visible = true;
            uiTitlePanel1.Visible = true;
            dataGrid(uiDataGridView1);
            uiLabel2.Text = "";
            lbMusicName.Text = "";
            axWindowsMediaPlayer1.StatusChange += axWindowsMediaPlayer1_StatusChange;

            uiPanel5.BringToFront();
            uiSymbolButton8.BringToFront();
            uiSymbolButton9.BringToFront();
            uiSymbolButton10.BringToFront();
        }

        public void dataGrid(UIDataGridView GridView)
        {
            GridView.CellBorderStyle = DataGridViewCellBorderStyle.None;

            //设置dataGridView为选中一行模式
            GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            //dataGridView中的标题居中显示
            GridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dataGridView的列居中
            int nu = GridView.ColumnCount;
            for (int i = 0; i < nu; i++)
            {
                GridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                GridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void uiSmoothLabel()
        {
            uiSmoothLabel1.BringToFront();
            uiSmoothLabel2.BringToFront();
            uiSmoothLabel3.BringToFront();
            uiSmoothLabel4.BringToFront();
            uiSmoothLabel5.BringToFront();
        }
        List<Songs> slist = new List<Songs>();
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <5; i++)
            { 
                pppp = HttpGet("https://www.yuanxiapi.cn/api/wangyiyun/?sort=新歌榜&type=json");
                var obj = JsonConvert.DeserializeObject<JObject>(pppp);
                Songs songs = new Songs();
                if (obj["code"].ToString() == "200")
                {
                    if (!obj["music"].ToString().Contains("404"))
                    {
                       
                        songs.songName = obj["name"].ToString();//歌曲名
                        songs.songId = obj["id"].ToString();//歌曲ID
                        songs.singer = obj["singer"].ToString();//歌手
                        songs.picture = obj["Picture"].ToString();//歌曲图片
                        songs.songUrl = obj["music"].ToString();//播放地址
                        songs.msg = obj["msg"].ToString();
                        songs.code = obj["code"].ToString();
                        axWindowsMediaPlayer1.URL = songs.songUrl;
                        songs.songType = axWindowsMediaPlayer1.currentMedia.getItemInfo("filetype");
                        slist.Add(songs);
                    }
                    else
                    {
                        UIMessageTip.ShowError("已自动跳过收费歌曲", 1500);
                    }
                }
                else
                {
                    UIMessageTip.ShowError("服务器异常", 5000);
                }
            }
            //var slist1 = slist.AsQueryable().ToList();
            //var slist2 = slist.FindIndex(j=>j.songName==slist[0].songName);

            uiDataGridView1.DataSource = slist;
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            lbMusicName.Text = "";
            uiLabel5.Text = "歌曲数：" + slist.Count;
            string Month = DateTime.Now.Month.ToString();
            string Date = DateTime.Now.Day.ToString();
            uiLabel4.Text = "最近更新：" + Month + "月" + Date + "日";



            //string[] ssss = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + @"\Temp\SongsPath.ini");
            //if (ssss.Length != 0)
            //{
            //    DirectoryInfo TheFolder = new DirectoryInfo(ssss[0]);
            //    foreach (FileInfo NextFile in TheFolder.GetFiles())
            //    {
            //        listBoxMusics.Items.Add(NextFile.Name);
            //        urlList.Add(ssss[0] + NextFile.Name);
            //    }
            //}
            //axWindowsMediaPlayer1.URL = @"E:\音乐\mp3\千千阙歌.mp3";
            uiTrackBar1.Value = 100;
            uiTabControlMenu1.Dock = DockStyle.Fill;
            axWindowsMediaPlayer1.BringToFront();
            axWindowsMediaPlayer1.settings.volume = uiTrackBar1.Value;
            uiSmoothLabel();
            //uiListBox1.BringToFront();
        }

        //每日精选
        List<Songs> son1 = new List<Songs>();
        List<Songs> son2 = new List<Songs>();
        List<Songs> son3 = new List<Songs>();
        List<Songs> son4 = new List<Songs>();
        string pppp;
        public void DailyPicks(List<Songs> slist, string SongList, UIListBox listbox)
        {
            slist.Clear();
            for (int i = 0; i < 15; i++)
            {
                pppp = HttpGet("https://www.yuanxiapi.cn/api/wangyiyun/?sort=" + SongList + "&type=json");
                var obj = JsonConvert.DeserializeObject<JObject>(pppp);
                Songs song = new Songs();
                if (obj["code"].ToString() == "200")
                {
                    if (!obj["music"].ToString().Contains("404"))
                    {
                        song.songName = obj["name"].ToString();//歌曲名
                        song.songId = obj["id"].ToString();//歌曲ID
                        song.singer = obj["singer"].ToString();//歌手
                        song.picture = obj["Picture"].ToString();//歌曲图片
                        song.songUrl = obj["music"].ToString();//播放地址
                        slist.Add(song);
                    }
                    else
                    {
                        UIMessageTip.ShowError("已自动跳过收费歌曲", 1500);
                    }
                }
                else
                {
                    UIMessageTip.ShowError("服务器异常", 5000);
                }
            }
            listbox.DataSource = slist;
            listbox.DisplayMember = "songName";
            listbox.ValueMember = "songId";
        }

        public void uiAvatar(List<Songs> list, UIListBox lis, string img)
        {
            var s = list.Where(i => i.songId == lis.SelectedValue.ToString()).ToList();
            uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(img).GetResponse().GetResponseStream()); ;
            uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(s[0].picture).GetResponse().GetResponseStream());
        }

        //播放
        private void btnPlay_Click(object sender, EventArgs e)
        {
            //声明变量
            int selectIndexList = listBoxMusics.SelectedIndex;
            if (selectIndexList >= 0)
            {
                if (btnPlay.Symbol == 61515)
                {
                    btnPlay.Symbol = 61516;
                    //获取文件的时间长度
                    max = axWindowsMediaPlayer1.currentMedia.duration;
                    //获取当前歌曲的播放位置
                    min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                    //类型强制转换
                    trackBar1.Maximum = (int)(max);
                    trackBar1.Value = (int)(min);

                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    var li = slist.Where(j => j.songId == listBoxMusics.SelectedValue.ToString()).ToList();
                    lbMusicName.Text = li[0].songName;
                    this.Text =li[0].songName;
                    uiLabel6.Text = axWindowsMediaPlayer1.currentMedia.durationString.ToString();
                }
                else
                {
                    btnPlay.Symbol = 61515;
                    //获取文件的时间长度
                    max = axWindowsMediaPlayer1.currentMedia.duration;
                    //获取当前歌曲的播放位置
                    min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                    //类型强制转换
                    trackBar1.Maximum = (int)(max);
                    trackBar1.Value = (int)(min);
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    var li = slist.Where(j => j.songId == listBoxMusics.SelectedValue.ToString()).ToList();
                    lbMusicName.Text = li[0].songName;
                    this.Text =li[0].songName;
                    uiLabel6.Text = axWindowsMediaPlayer1.currentMedia.durationString.ToString();
                }






                ////获取文件的时间长度
                //max = axWindowsMediaPlayer1.currentMedia.duration;
                //    //获取当前歌曲的播放位置
                //    min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                //    //类型强制转换
                //    trackBar1.Maximum = (int)(max);
                //    trackBar1.Value = (int)(min);

                //    axWindowsMediaPlayer1.Ctlcontrols.play();
                //    lbMusicName.Text = listBoxMusics.SelectedItem.ToString();
                //    this.Text = "千千音乐-" + listBoxMusics.SelectedItem.ToString();


                //判断列表中是否有选中的歌曲，有的话播放选中的，没有就播放第一首
                //selectIndexList = selectIndexList < 0 ? 0 : selectIndexList;
                ////更新选中行 重新设置当前索引
                //listBoxMusics.SelectedIndex = selectIndexList;
                //把urlList中存储的url地址赋给播放器控件
                //axWindowsMediaPlayer1.URL = urlList[selectIndexList];

            }
            else if (selectIndexList == -1)
            {
                UIMessageTip.ShowWarning("请先添加音乐");
            }
            else
            {
                listBoxMusics.SelectedIndex = 0;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            timer1.Enabled = true;
        }

        //暂停
        private void btnPause_Click(object sender, EventArgs e)
        {
            //获取文件的时间长度
            max = axWindowsMediaPlayer1.currentMedia.duration;
            //获取当前歌曲的播放位置
            min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            //类型强制转换
            trackBar1.Maximum = (int)(max);
            trackBar1.Value = (int)(min);
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }
        //停止
        private void btnStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            //停止后禁用进度条
            timer1.Enabled = false;
            //this.Text = "千千音乐";
            //lbMusicName.Text = "千千音乐";
            trackBar1.Value = 0;
            btnPlay.Symbol = 61515;
        }
        //上一首
        private void btnLast_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            if (listBoxMusics.SelectedIndex > 0)
            {
                int selectIndexList = listBoxMusics.SelectedIndex - 1;
                selectIndexList = selectIndexList < 0 ? 0 : selectIndexList;//三目运算符
                 //更新选中行 重新设置当前索引
                listBoxMusics.SelectedIndex = selectIndexList;
                var songs = son1.Where(i => i.songId == listBoxMusics.SelectedValue.ToString()).ToList();
                axWindowsMediaPlayer1.URL = songs[0].songUrl;
                lbMusicName.Text = songs[0].songName;
                this.Text = songs[0].songName;
                uiLabel2.Text = songs[0].singer;
                trackBar1.Value = 0;
                uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
                uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
            }
            else
            {
                trackBar1.Value = 0;
                listBoxMusics.SelectedIndex = listBoxMusics.Count-1;
                var songs = son1.Where(i => i.songId == listBoxMusics.SelectedValue.ToString()).ToList();
                axWindowsMediaPlayer1.URL = songs[0].songUrl;
                lbMusicName.Text = songs[0].songName;
                this.Text = songs[0].songName;
                uiLabel2.Text = songs[0].singer;
                trackBar1.Value = 0;
                uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
                uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
            }
        }
        //下一首
        private void btnNext_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            if (listBoxMusics.SelectedIndex >= 0)
            {
                int selectIndexList = listBoxMusics.SelectedIndex + 1;
                selectIndexList = selectIndexList < 0 ? 0 : selectIndexList;//三目运算符
                                                                            //更新选中行 重新设置当前索引
                if (selectIndexList == listBoxMusics.Count)
                {
                    listBoxMusics.SelectedIndex = 0;
                    //listBoxMusics.SelectedIndex = selectIndexList;
                    var songs = son1.Where(i => i.songId == listBoxMusics.SelectedValue.ToString()).ToList();
                    axWindowsMediaPlayer1.URL = songs[0].songUrl;
                    lbMusicName.Text = songs[0].songName;
                    uiLabel2.Text = songs[0].singer;
                    this.Text =songs[0].songName;
                    trackBar1.Value = 0;
                    uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
                    uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
                }
                else
                {
                    trackBar1.Value = 0;
                    listBoxMusics.SelectedIndex = selectIndexList;
                    var songs = son1.Where(i => i.songId == listBoxMusics.SelectedValue.ToString()).ToList();
                    axWindowsMediaPlayer1.URL = songs[0].songUrl;
                    lbMusicName.Text = songs[0].songName;
                    this.Text = songs[0].songName;
                    uiLabel2.Text = songs[0].singer;
                    trackBar1.Value = 0;
                    uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
                    uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
                }
            }
            


            ////LIstBox中的索引和URLList中的索引相对应
            ////获取当前选中歌曲的索引
            //if (listBoxMusics.SelectedIndex >= 0)
            //{
            //    int selectIndexList = listBoxMusics.SelectedIndex + 1;
            //    selectIndexList = selectIndexList == listBoxMusics.Items.Count ? listBoxMusics.SelectedIndex : selectIndexList;//三目运算符
            //                                                                                                                   //更新选中行 重新设置当前索引
            //    listBoxMusics.SelectedIndex = selectIndexList;
            //    //把urlList中存储的url地址赋给播放器控件
            //    axWindowsMediaPlayer1.URL = urlList[selectIndexList];
            //    lbMusicName.Text = listBoxMusics.SelectedItem.ToString();
            //    this.Text = "千千音乐-" + listBoxMusics.SelectedItem.ToString();
            //    trackBar1.Value = 0;
            //}
        }

        //
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //获取文件的时间长度
                max = axWindowsMediaPlayer1.currentMedia.duration;
                //获取当前歌曲的播放位置
                min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                //类型强制转换
                trackBar1.Maximum = (int)(max);
                trackBar1.Value = (int)(min);

                //一首播放完成后继续下一首
                if (axWindowsMediaPlayer1.playState == WMPPlayState.wmppsStopped)
                {
                    //当前歌曲结束播放后获取下一首歌曲的索引值
                    int selectIndex = listBoxMusics.SelectedIndex + 1;
                    selectIndex = selectIndex == listBoxMusics.Items.Count ? 0 : selectIndex;//三目运算符
                    axWindowsMediaPlayer1.URL = urlList[selectIndex];
                    listBoxMusics.SelectedIndex = selectIndex;
                    lbMusicName.Text = listBoxMusics.SelectedItem.ToString();
                    this.Text =listBoxMusics.SelectedItem.ToString();
                    //lbMusicName.BackColor = Red;
                    trackBar1.Value = 0;
                    trackBar1.Enabled = true;
                }
            }
            catch
            {

            }
        }

        //鼠标拖动进度条时间
        //鼠标按下
        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            //暂停播放
            timer1.Enabled = false;
            axWindowsMediaPlayer1.Ctlcontrols.pause();

        }

        private void Form1_MaximumSizeChanged(object sender, EventArgs e)
        {

        }
        static string lurl = "";
        static string lrcUrl = lurl;
        string[] nameList;
        private void btnInput_Click_1(object sender, EventArgs e)
        {
            //实例化一个打开文件的对话框
            OpenFileDialog of = new OpenFileDialog();
            //让选择器可以多选文件
            of.Multiselect = true;
            of.Title = "请选择音乐文件";
            //指定文件的类型
            of.Filter = "(*.mp3)|*.mp3";
            //of.ShowDialog();
            //确认用户选择的是确定按钮
            if (of.ShowDialog() == DialogResult.OK)
            {
                //把用户选择的数据存储到数组中
                nameList = of.FileNames;
                //读取数组中的数据
                foreach (string url in nameList)
                {
                    //lurl = Path.GetFileNameWithoutExtension(url);
                    lurl = Path.GetFileNameWithoutExtension(url);//获取不具有扩展名的文件

                    listBoxMusics.Items.Add(Path.GetFileNameWithoutExtension(url));
                    urlList.Add(url);
                    lurl = url;
                }
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Temp\SongsPath.ini"))
                {
                    File.WriteAllText(System.Windows.Forms.Application.StartupPath + @"\Temp\SongsPath.ini", Path.GetDirectoryName(lurl) + @"\");
                }

                listBoxMusics.Visible = true;
                uiTitlePanel1.Visible = true;
                listBoxMusics.SelectedIndex = 0;
            }
        }

        //列表选择发生变化时播放选择的歌曲
        private void listBoxMusics_ItemDoubleClick(object sender, EventArgs e)
        {
            var songs = son1.Where(i => i.songId == listBoxMusics.SelectedValue.ToString()).ToList();
            //LIstBox中的索引和URLList中的索引相对应
            //获取当前选中歌曲的索引
            int selectIndexList = listBoxMusics.SelectedIndex;
            //把urlList中存储的url地址赋给播放器控件
            //axWindowsMediaPlayer1.URL = urlList[selectIndexList];
            axWindowsMediaPlayer1.URL = songs[0].songUrl;
            uiLabel6.Text = axWindowsMediaPlayer1.currentMedia.durationString.ToString();
            //lbMusicName.Text = listBoxMusics.SelectedItem.ToString();
            lbMusicName.Text = songs[0].songName;
            //this.Text = "千千音乐-" + listBoxMusics.SelectedItem.ToString();
            this.Text = songs[0].songName;


            uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
            uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(songs[0].picture).GetResponse().GetResponseStream());
            timer1.Enabled = true;
            btnPlay.Symbol = 61516;
            //ShowLrc();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            //if (listBoxMusics.Visible)
            //{
            //    uiTitlePanel1.Visible = false;
            //    listBoxMusics.Visible = false;
            //    uiTabControlMenu1.Dock = DockStyle.Fill;
            //}
            //else
            //{
            //    uiPanel1.Visible = false;
            //    uiTitlePanel1.BringToFront();
            //    listBoxMusics.BringToFront();
            //    listBoxMusics.Visible = true;
            //    uiTitlePanel1.Visible = true;
            //    uiTabControlMenu1.Dock = DockStyle.Left;
            //}
        }

        private void uiTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = uiTrackBar1.Value;
            axWindowsMediaPlayer1.settings.mute = false;
            uiSymbolButton2.Symbol = 361480;
            //toolTip1.ToolTipTitle = uiTrackBar1.Value.ToString();

        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            switch (uiSymbolButton2.Symbol)
            {
                case 361480:
                    axWindowsMediaPlayer1.settings.mute = true;
                    uiTrackBar1.Value = 0;
                    if (uiTrackBar1.Value == 0) {
                        uiSymbolButton2.Symbol = 363145;
                    }
                    break;
                case 363145:
                    axWindowsMediaPlayer1.settings.mute = false;
                    uiTrackBar1.Value =100;
                    if (uiTrackBar1.Value == 100)
                    {
                        uiSymbolButton2.Symbol = 361480;
                    }
                    break;
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            //if (uiComboBox1.Text.Trim() != "")
            //{
            //    using (var client = new WebClient())
            //    {
            //        client.Encoding = Encoding.UTF8;
            //        string serviceAddress = "https://v2.alapi.cn/api/music/search?keyword=" + uiComboBox1.Text+ "&token=LwExDtUWhF3rH5ib";
            //        //string serviceAddress = "http://api.tianapi.com/hotreview/index?key=fb6637b44f5d255b8559eedf64cb0711";//请求URL地址
            //        var data = client.DownloadString(serviceAddress);
            //        var obj = JsonConvert.DeserializeObject<JObject>(data);
            //        for (int i = 0; i < obj.Count; i++)
            //        {
            //            s.songName = obj["data"]["songs"][i]["name"].ToString();
            //            s.songId = obj["data"]["songs"][i]["id"].ToString();
            //            songs.Add(s);
            //        }
            //    }
            //}
        }

        private void uiListBox1_ItemClick(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {

        }
        string[,] lrc = new string[2, 500];//保存歌词和当前进度
        private void uiAvatar1_Click(object sender, EventArgs e)
        {
            uiPanel1.Visible = false;
            //uiTitlePanel1.Visible = false;
            //listBoxMusics.Visible = false;
            uiTabControlMenu1.Visible = false;
            uiPanel3.Visible = true;


            uiSymbolLabel1.Visible = true;
            uiPanel3.Dock = DockStyle.Fill;
            uiPanel3.BringToFront();
            uiPanel3.BackColor = System.Drawing.Color.Transparent;

        }

        public void ShowLrc()
        {
            string aaaa = lrcUrl;
            string bbbb = aaaa;
            //using:作用是使用完成后自动释放内存
            //StreamReader:作用是用特定的编码从字节流中读取字节
            StreamReader sr = new StreamReader(@"F:\音乐\KwDownload\song\DJ Candy-千千阙歌2版.lrc", Encoding.Default);
            String line = "123";
            //循环读取每一行歌词
            while ((line = sr.ReadLine()) != null)
            {
                //将读取到的歌词存放到数组中
                for (int i = 0; i < 500; i++)
                {
                    if (lrc[0, i] != null)
                    {
                        lrc[0, i] = line.Substring(10, line.Length);
                        break;
                    }
                }
                //将读取到的歌词时间存放到数组中
                for (int i = 0; i < 500; i++)
                {
                    if (lrc[1, i] == null)
                    {
                        lrc[1, i] = line.Substring(1, 5);
                        break;
                    }
                }
            }
            /***********动态显示歌词***************/
            //获取播放器当前进度
            //string numss = this.axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            //for (int i = 0; i < 500; i++)
            //{
            //    if (lrc[1, i].Equals(numss))
            //    {
            //        this.uiLabel1.Text = lrc[0, i];
            //    }
            //    else
            //    {
            //        this.uiLabel1.Text = "************";
            //    }
            //}
            //using ()
            //{

            //}

            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("异常：" + ex.Message);
            //}
        }
        /// <summary>
        /// 刷新歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer5_Tick(object sender, EventArgs e)
        {
            ShowLrc();
        }

        private void uiSymbolLabel1_Click(object sender, EventArgs e)
        {
            uiPanel3.Visible = false;
            uiSymbolLabel1.Visible = false;
            uiTabControlMenu1.Visible = true;
            uiPanel3.SendToBack();
            uiPanel3.Dock = DockStyle.None;
        }

        private void 清空列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxMusics.Items.Clear();
        }

        public static string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {
            if (search_tb.Text.Trim() != "")
            {
                string wangyi = "https://music.163.com/#/search/m/?s=" + search_tb.Text + "";
                string kugou = "http://msearchcdn.kugou.com/api/v3/search/song?keyword=" + search_tb.Text + "&pagesize=2";
                //string api = "https://v2.alapi.cn/api/music/search?keyword=" + search_tb.Text + "&token=LwExDtUWhF3rH5ib";
                string pppp = HttpGet(wangyi);
                var obj = JsonConvert.DeserializeObject<JObject>(pppp);
                var ooo = obj;
                if (obj["code"].ToString() == "102")
                {
                    UIMessageTip.ShowWarning("已达到API请求上限", 2000);
                }
                else
                {

                }
            }
        }

        public void selectSong(List<Songs> slist, UIListBox listbox)
        {
            var s = slist.Where(i => i.songId == listbox.SelectedValue.ToString()).ToList();
            axWindowsMediaPlayer1.URL = s[0].songUrl;
            lbMusicName.Text = s[0].songName;
            uiLabel2.Text = s[0].singer;
            this.Text = s[0].songName;
            uiAvatar(slist, listbox, s[0].picture);
            timer1.Enabled = true;
            btnPlay.Symbol = 61516;
        }

        //窗体激活后在加载榜单列表
        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
        }

        private void uiPanel7_MouseLeave(object sender, EventArgs e)
        {
            //btn_Left.Visible = false; btn_right.Visible = false;
        }

        private void uiTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            string Index = uiTabControl1.SelectedIndex.ToString();
            switch (Index)
            {
                case "0":

                    break;

                case "1":

                    break;

                case "2":

                    break;

                case "3":

                    break;

                case "4":

                    break;
                default:
                    break;
            }
        }

        private void uiDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = uiDataGridView1.SelectedCells[0].RowIndex;//获取所在行
            string song = uiDataGridView1.Rows[index].Cells[4].Value.ToString();
            string url = uiDataGridView1.Rows[index].Cells[5].Value.ToString();
            string singer = uiDataGridView1.Rows[index].Cells[7].Value.ToString();
            string picture = uiDataGridView1.Rows[index].Cells[8].Value.ToString();
            string id = uiDataGridView1.Rows[index].Cells[3].Value.ToString();
            axWindowsMediaPlayer1.URL = url;
            lbMusicName.Text = song;
            uiLabel2.Text = singer;
            this.Text = song;
            uiAvatar1.Image = Image.FromStream(System.Net.WebRequest.Create(picture).GetResponse().GetResponseStream());
            uiAvatar2.Image = Image.FromStream(System.Net.WebRequest.Create(picture).GetResponse().GetResponseStream());
            timer1.Enabled = true;
            btnPlay.Symbol = 61516;
            
            Songs ss = new Songs();
            ss.songId = id;
            ss.songName = song;
            ss.songUrl = url;
            ss.picture = picture;
            ss.singer = singer;
            var s = son1.Where(i => i.songId == id).ToList();
            if (s.Count == 0)
            {
                son1.Add(ss);
            }
            listBoxMusics.DataSource = son1.AsQueryable().ToList();
            listBoxMusics.DisplayMember = "songName";
            listBoxMusics.ValueMember = "songId";
            listBoxMusics.SelectedIndex = listBoxMusics.Items.Count - 1;

            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Temp\SongsPath.ini"))
            {
                StreamReader sr = new StreamReader(System.Windows.Forms.Application.StartupPath + @"\Temp\SongsPath.ini", Encoding.UTF8);
                string[] ru = sr.ReadToEnd().Split('x', '\r', '\n');
                sr.Close();
                sr.Dispose();
                string nnn = "" + song + "x" + id + "x" + picture + "x" + url + "\r\n";
                if (!ru.Contains(nnn))
                {
                    StreamWriter writer = File.AppendText(System.Windows.Forms.Application.StartupPath + @"\Temp\SongsPath.ini");
                    writer.Write(nnn);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        private void uiSymbolButton7_Click(object sender, EventArgs e)
        {
            if (uiPanel5.Visible) {
                uiPanel5.Visible = false;
                uiSymbolButton8.Visible = false;
                uiSymbolButton9.Visible = false;
                uiSymbolButton10.Visible = false;
            }
            else
            {
                uiPanel5.Visible = true;
                uiSymbolButton8.Visible = true;
                uiSymbolButton9.Visible = true;
                uiSymbolButton10.Visible = true;
                uiPanel5.BringToFront();
                uiSymbolButton8.BringToFront();
                uiSymbolButton9.BringToFront();
                uiSymbolButton10.BringToFront();
            }
            //switch (uiSymbolButton7.Symbol)
            //{
            //    case 362764:
            //        UIMessageTip.ShowOk("顺序播放");
            //        PlayState("shuffle",false);
            //        uiSymbolButton7.Symbol = 362764;
            //        break;
            //    case 362306:
            //        UIMessageTip.ShowOk("单曲循环");
            //        PlayState("loop",true);
            //        uiSymbolButton7.Symbol = 362306;
            //        break;
            //    case 361556:
            //        UIMessageTip.ShowOk("随机播放");
            //        PlayState("shuffle",true);
            //        uiSymbolButton7.Symbol = 361556;
            //        break;
            //    default:
            //        break;
            //}
        }

        //鼠标抬起
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            //获取被拖动以后的位置
            double doValue = trackBar1.Value;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = doValue;//重置播放位置
            axWindowsMediaPlayer1.Ctlcontrols.play();
            timer1.Enabled = true;
        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            var li = slist.Where(j => j.songId == listBoxMusics.SelectedValue.ToString()).ToList();
            switch ((int)axWindowsMediaPlayer1.playState)
            {
                case 1:
                    UIMessageTip.Show("已停止");
                    break;
                case 2:
                    UIMessageTip.Show("已暂停");
                    break;
                case 3:
                    //UIMessageTip.Show("播放中");
                    uiLabel6.Text = axWindowsMediaPlayer1.currentMedia.durationString.ToString();
                    lbMusicName.Text = "正在播放：" + li[0].songName;
                    break;
                case 4:
                    UIMessageTip.Show("向前搜索");
                    break;
                case 5:
                    UIMessageTip.Show("向后搜索");
                    break;
                case 6:
                    //UIMessageTip.Show("正在缓冲");
                    lbMusicName.Text = "缓冲中....";
                    break;
                case 7:
                    UIMessageTip.Show("正在等待流开始");
                    break;
                case 8:
                    //UIMessageTip.Show("播放流已结束");
                    lbMusicName.Text = "";
                    uiLabel2.Text = "";
                    btnPlay.Symbol = 61515;
                    break;
                case 9:
                    lbMusicName.Text = "";
                    //UIMessageTip.Show("正在连接");
                    break;
                case 10:
                    lbMusicName.Text = "";
                    //lbMusicName.Text = "准备就绪";
                    trackBar1.Value = 0;
                    break;
                default:
                    break;
            }
        }

        private void uiSymbolButton9_Click(object sender, EventArgs e)
        {
            uiSymbolButton7.Symbol = 361556;
            uiPanel5.Visible = false;
            uiSymbolButton8.Visible = false;
            uiSymbolButton9.Visible = false;
            uiSymbolButton10.Visible = false;
            
        }

        private void uiSymbolButton8_Click(object sender, EventArgs e)
        {
            uiSymbolButton7.Symbol = 362306;
            uiPanel5.Visible = false;
            uiSymbolButton8.Visible = false;
            uiSymbolButton9.Visible = false;
            uiSymbolButton10.Visible = false;
        }

        private void uiSymbolButton10_Click(object sender, EventArgs e)
        {
            uiSymbolButton7.Symbol = 362764;
            uiPanel5.Visible = false;
            uiSymbolButton8.Visible = false;
            uiSymbolButton9.Visible = false;
            uiSymbolButton10.Visible = false;
        }

        public void PlayState(string state,bool isok)
        {
            axWindowsMediaPlayer1.settings.setMode(state, isok);
        }
    }
}
