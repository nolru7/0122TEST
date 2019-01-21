using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _20181207.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _20181207.Controller
{
    class MainController
    {
        private Commons comm;
        private Panel head, contents, view, controller;
        private Button btn1, btn2, btn3;
        private ListView listView;
        private TextBox textBox1, textBox2, textBox3, textBox4, textBox5, textBox6;
        private Form parentForm, tagetForm;
        private Hashtable hashtable;
        private string nNo = null;
        private bool first = true;

        public MainController(Form parentForm)
        {
            this.parentForm = parentForm;
            comm = new Commons();
            getView();
        }

        private void getView()
        {
            hashtable = new Hashtable();
            hashtable.Add("sX", 1000);
            hashtable.Add("sY", 100);
            hashtable.Add("pX", 0);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 1);
            hashtable.Add("name", "head");
            head = comm.getPanel(hashtable, parentForm);

            hashtable = new Hashtable();
            hashtable.Add("sX", 1000);
            hashtable.Add("sY", 700);
            hashtable.Add("pX", 0);
            hashtable.Add("pY", 100);
            hashtable.Add("color", 4);
            hashtable.Add("name", "contents");
            contents = comm.getPanel(hashtable, parentForm);

            hashtable = new Hashtable();
            hashtable.Add("sX", 1000);
            hashtable.Add("sY", 20);
            hashtable.Add("pX", 0);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 3);
            hashtable.Add("name", "controller");
            controller = comm.getPanel(hashtable, contents);

            hashtable = new Hashtable();
            hashtable.Add("sX", 1000);
            hashtable.Add("sY", 680);
            hashtable.Add("pX", 0);
            hashtable.Add("pY", 20);
            hashtable.Add("color", 0);
            hashtable.Add("name", "view");
            view = comm.getPanel(hashtable, contents);

            hashtable = new Hashtable();
            hashtable.Add("sX", 200);
            hashtable.Add("sY", 80);
            hashtable.Add("pX", 100);
            hashtable.Add("pY", 10);
            hashtable.Add("color", 0);
            hashtable.Add("name", "btn1");
            hashtable.Add("text", "입력");
            hashtable.Add("click", (EventHandler) SetInsert);
            btn1 = comm.getButton(hashtable, head);

            hashtable = new Hashtable();
            hashtable.Add("sX", 200);
            hashtable.Add("sY", 80);
            hashtable.Add("pX", 400);
            hashtable.Add("pY", 10);
            hashtable.Add("color", 0);
            hashtable.Add("name", "btn2");
            hashtable.Add("text", "수정");
            hashtable.Add("click", (EventHandler) SetUpdate);
            btn2 = comm.getButton(hashtable, head);

            hashtable = new Hashtable();
            hashtable.Add("sX", 200);
            hashtable.Add("sY", 80);
            hashtable.Add("pX", 700);
            hashtable.Add("pY", 10);
            hashtable.Add("color", 0);
            hashtable.Add("name", "btn3");
            hashtable.Add("text", "삭제");
            hashtable.Add("click", (EventHandler) SetDelete);
            btn3 = comm.getButton(hashtable, head);

            hashtable = new Hashtable();
            hashtable.Add("color", 0);
            hashtable.Add("name", "listView");
            hashtable.Add("click", (MouseEventHandler)listView_click);
            listView = comm.getListView(hashtable, view);

            hashtable = new Hashtable();
            hashtable.Add("width", 45);
            hashtable.Add("pX", 0);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 0);
            hashtable.Add("name", "textBox1");
            hashtable.Add("enabled", false);
            textBox1 = comm.getTextBox(hashtable, controller);

            hashtable = new Hashtable();
            hashtable.Add("width", 100);
            hashtable.Add("pX", 45);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 0);
            hashtable.Add("name", "textBox2");
            hashtable.Add("enabled", true);
            textBox2 = comm.getTextBox(hashtable, controller);

            hashtable = new Hashtable();
            hashtable.Add("width", 350);
            hashtable.Add("pX", 145);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 0);
            hashtable.Add("name", "textBox3");
            hashtable.Add("enabled", true);
            textBox3 = comm.getTextBox(hashtable, controller);

            hashtable = new Hashtable();
            hashtable.Add("width", 100);
            hashtable.Add("pX", 495);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 0);
            hashtable.Add("name", "textBox4");
            hashtable.Add("enabled", true);
            textBox4 = comm.getTextBox(hashtable, controller);

            hashtable = new Hashtable();
            hashtable.Add("width", 200);
            hashtable.Add("pX", 595);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 0);
            hashtable.Add("name", "textBox5");
            hashtable.Add("enabled", false);
            textBox5 = comm.getTextBox(hashtable, controller);

            hashtable = new Hashtable();
            hashtable.Add("width", 200);
            hashtable.Add("pX", 795);
            hashtable.Add("pY", 0);
            hashtable.Add("color", 0);
            hashtable.Add("name", "textBox6");
            hashtable.Add("enabled", false);
            textBox6 = comm.getTextBox(hashtable, controller);

            GetSelect();
        }

        private void GetSelect()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            listView.Clear();

            listView.Columns.Add("nNo", 45, HorizontalAlignment.Center);        /* Notice 번호 */
            listView.Columns.Add("nTitle", 100, HorizontalAlignment.Left);      /* Notice 제목 */
            listView.Columns.Add("rContents", 350, HorizontalAlignment.Left);   /* Notice 내용 */
            listView.Columns.Add("mName", 100, HorizontalAlignment.Center);     /* Notice 작성자 이름 */
            listView.Columns.Add("regDate", 200, HorizontalAlignment.Left);     /* Notice 작성 현재날짜 */
            listView.Columns.Add("modDate", 200, HorizontalAlignment.Left);     /* Notice 수정 현재날짜 */

            // 보여 주기 가상 데이터 -> WebAPI를 이용하여 데이터 가져올것!
            //listView.Items.Add(new ListViewItem(new string[] { "3", "제목3", "내용3", "Winform", "2018-12-07", "2016-12-07" }));
            //listView.Items.Add(new ListViewItem(new string[] { "2", "제목2", "내용2", "스마트", "2018-12-06", "2016-12-07" }));
            //listView.Items.Add(new ListViewItem(new string[] { "1", "제목1", "내용1", "관리자", "2018-12-05", "2016-12-07" }));

            WebApi("http://192.168.3.220:5000/api/Select", nNo);

        }

                


        private void SetInsert(object o, EventArgs e)
        {
            MessageBox.Show("SetInsert");

            if(textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("입력을 확인해주세요.");               
            }
            else
            {
                WebApi("http://192.168.3.220:5000/api/Insert", nNo);
            }
            

        }
        private void SetUpdate(object o, EventArgs e)
        {
            MessageBox.Show("SetUpdate");

            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("입력을 확인해주세요.");
            }
            else
            {
                WebApi("http://192.168.3.220:5000/api/Update", nNo);
            }

        }
        private void SetDelete(object o, EventArgs e)
        {
            MessageBox.Show("SetDelete");

            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("입력을 확인해주세요.");
            }
            else
            {
                WebApi("http://192.168.3.220:5000/api/Delete", nNo);
            }

            
        }
        private void listView_click(object o, EventArgs a)
        {
            MessageBox.Show("listView_click");

            int index;
            index = listView.FocusedItem.Index;  // 선택돈 아이템 인덱스 번호 얻기
            //MessageBox.Show("선택한 행의 인덱스 번호 : " + index.ToString());
            nNo = listView.Items[index].SubItems[0].Text; // 인덱스 번호의 n번째 아이템 얻기
            //MessageBox.Show("선택한 행의 0번째 아이템 : " + nNo);

            textBox2.Text = listView.Items[index].SubItems[1].Text;
            textBox3.Text = listView.Items[index].SubItems[2].Text;
            textBox4.Text = listView.Items[index].SubItems[3].Text;

        }


        private void listView_Clear()
        {
            listView.Clear();
            listView.Columns.Add("nNo", 45, HorizontalAlignment.Center);        /* Notice 번호 */
            listView.Columns.Add("nTitle", 100, HorizontalAlignment.Left);      /* Notice 제목 */
            listView.Columns.Add("rContents", 350, HorizontalAlignment.Left);   /* Notice 내용 */
            listView.Columns.Add("mName", 100, HorizontalAlignment.Center);     /* Notice 작성자 이름 */
            listView.Columns.Add("regDate", 200, HorizontalAlignment.Left);     /* Notice 작성 현재날짜 */
            listView.Columns.Add("modDate", 200, HorizontalAlignment.Left);     /* Notice 수정 현재날짜 */
        }



        private void WebApi(string apiurl, string nNo)
        {
            WebClient client = new WebClient(); // 웹 접속 객체 생성
            NameValueCollection data = new NameValueCollection(); // 파라메터값 담을 객체 생성

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"); // 웹 호출 시 보낸쪽 정보 설정
            client.Encoding = Encoding.UTF8; // UTF-8 설정 하여 한글 처리하기

            //string url = "http://192.168.3.220:5000/api/Insert"; // 웹 호출 주소 정의하기
            string url = apiurl; // 웹 호출 주소 정의하기
            string method = "POST"; // 웹 호출 시 통신 방식 정의하기

            //MessageBox.Show(textBox2.Text + " " + textBox3.Text + " " + textBox4.Text);
            //MessageBox.Show("nNo : " + nNo);

            data.Add("nTitle", textBox2.Text); // 파라메터값 정의하기 (key, value) 형식
            data.Add("nContents", textBox3.Text);
            data.Add("mName", textBox4.Text);
            if(nNo != null) { data.Add("nNo", nNo); }            

            byte[] result = client.UploadValues(url, method, data); // 데이터 호출 후 Byte[] 값 받기
            string strResult = Encoding.UTF8.GetString(result); // 리턴 받은 Byte[] 값를 문자열로 형변환 하기
            //Console.WriteLine(strResult); // 문자열 데이터 확인하기.

            //if(strResult == "[]")
            //{
            //    MessageBox.Show("입력을 확인해주세요.");
            //}
            //else
            //{
                //MessageBox.Show(strResult);
                listView_Clear();

                ArrayList jList = JsonConvert.DeserializeObject<ArrayList>(strResult); // JSON 데이터 변경
                ArrayList list = new ArrayList(); // JSON 에서 LIST로 담을 객체 생성            
                foreach (JObject row in jList) // JSON 데이터에서 key : value 형식으로 분리하기
                {
                    Hashtable ht = new Hashtable(); // Key : Value 형식으로 데이터 담을 객체 생성
                    foreach (JProperty col in row.Properties()) // JSON 속성 가져오기
                    {
                        //Console.Write("{0} :\t{1}\t", col.Name, col.Value); // Key : Value 형식으로 출력하기
                        ht.Add(col.Name, col.Value); // Key : Value 형식으로 데이터 담기
                    }
                    list.Add(ht); // JSON 에서 LIST 로 데이터 담기
                }



                foreach (Hashtable ht in list)
                {

                    ListViewItem item = new ListViewItem(ht["nNo"].ToString());
                    item.SubItems.Add(ht["nTitle"].ToString());
                    item.SubItems.Add(ht["nContents"].ToString());
                    item.SubItems.Add(ht["mName"].ToString());
                    item.SubItems.Add(ht["regDate"].ToString());
                    item.SubItems.Add(ht["modDate"].ToString());
                    listView.Items.Add(item);
                }
            //}
                        
        }
    }
}
