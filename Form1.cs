using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using Oracle.ManagedDataAccess.Client;

namespace webCrawling
{
    public partial class frmMTMProgram : Form
    {
        protected ChromeDriverService driverService = null;
        protected ChromeOptions options = null;
        protected ChromeDriver driver = null;
        protected Boolean isContainsId = false;
        protected String pageCurrentHandleBefore = "";
        string[] organizationList = new string[4] { "나이스", "한국", "KIS", "에프앤"};// 평가기관명
        string[] columnHeaderList = new string[20] {"KIND", "KINDNAME", "CREDITLEVEL", "", "TNRVAL_3M" , 
                                                    "TNRVAL_6M", "TNRVAL_9M", "TNRVAL_1Y", "TNRVAL_1Y6M", 
                                                    "TNRVAL_2Y", "TNRVAL_2Y6M", "TNRVAL_3Y", "TNRVAL_4Y", 
                                                    "TNRVAL_5Y", "TNRVAL_7Y", "TNRVAL_10Y", "TNRVAL_15Y", 
                                                    "TNRVAL_20Y", "TNRVAL_30Y", "TNRVAL_50Y" }; // 그리드의 필드명
        String[,] bondMarkToMarketList = new string[42,21];
        protected string tempStr = "";
        string[] bondMarkToMarketValueList = new string[42];
        protected string sSQL = "";
        OracleConnection pgOraConn;
        OracleCommand pgOraCmd;
        protected DataTable dtblTable = new DataTable(); // SQL 실행결과를 담아둘 DataTable

        // 파일 경로
        protected string donwloadPath = @"C:\Users\user\Downloads\채권시가평가기준수익률.xls";
        protected string copyPath = "";
        protected string logPath = Directory.GetCurrentDirectory() + @"\log\log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        protected string bakPath = "";
        

        // 개발계용 DB
        protected string sOracleServer = "10.0.1.41";
        protected string sOracleId = "PDV";
        protected string sOraclePassword = "pwd0719";
        protected string sOracleSevice = "DEVDB";

        // 운영계용 DB
        //static string sOracleServer = "10.0.1.30";
        //static string sOracleId = "nice";
        //static string sOraclePassword = "nps1260";
        //static string sOracleSevice = "PNIDB";

        protected StreamWriter streamWriter;

        ///<summary>
        /// Form Load 
        ///</summary>
        ///<remarks>
        ///</remarks>
        public frmMTMProgram()
        {
            InitializeComponent();

            try
            {
                streamWriter = File.CreateText(logPath) ;
                streamWriter.WriteLine(DateTime.Now + " | 작업 시작!");
                streamWriter.Close();

                driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;

                options = new ChromeOptions();
                options.AddArgument("--start-maximized"); // 전체 화면 옵션

                initGrid();

            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message);
            }
        }
        ///<summary>
        /// 크롤링 진행 버튼
        ///</summary>
        ///<remarks>
        ///</remarks>
        private void searchMTMData_Click(object sender, EventArgs e)
        {

            try
            {
                // 기준일자 사용 false
                standardTime.Enabled = false;

                streamWriter = File.AppendText(logPath);
                streamWriter.WriteLine(DateTime.Now + " | 조회 버튼 클릭!");
                streamWriter.Close();

                var todayDate = openUrlAndAccessFrame();

                crawlingData(todayDate);

            }
            catch (Exception exc)
            {
                driver.Quit();
                Trace.WriteLine(exc.ToString());
                Application.Exit();
            }

        }
        ///<summary>
        /// 프로그램 종료
        ///</summary>
        ///<remarks>
        ///</remarks>
        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (driver == null)
            {
               Application.Exit();
            }
            else
            {
               driver.Quit();
               Application.Exit();
            }
        }

        ///<summary>
        /// 크롤링 데이터 INSERT 
        ///</summary>
        ///<remarks>
        ///</remarks>
        private void insertMTMValue_Click(object sender, EventArgs e)
        {
            streamWriter = File.AppendText(logPath);
            streamWriter.WriteLine(DateTime.Now + " | insertMTMValue_Click(함수) --- 크롤링 데이터 DB 입력 시작! ");
            streamWriter.Close();

            sSQL = "";

            // Oracle 접속정보
            pgOraConn = new OracleConnection($"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + sOracleServer + ")(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + sOracleSevice + ")));User ID=" + sOracleId + ";Password=" + sOraclePassword + ";Connection Timeout=30;");
            // Oracle 접속
            pgOraConn.Open();
            pgOraCmd = pgOraConn.CreateCommand();

            string std_dt = DateTime.Now.ToString("yyyy-MM-dd");
            string groupName = "";
            string groupCode = "";
            string tnrCode = "";
            //string tnrValue = tnrVal;


            if (grdv_AVG_TNRVAL.RowCount > 0)
            {
                for(int rowIndex = 0; rowIndex < grdv_AVG_TNRVAL.RowCount; rowIndex++)
                {
                    for (int columnIndex = 4; columnIndex <= grdv_AVG_TNRVAL.Columns.Count; columnIndex++)
                    {
                        string tnrValue = grdv_AVG_TNRVAL.GetRowCellValue(rowIndex, columnHeaderList[columnIndex]) as string;
                        
                        if (tnrValue != "-")
                        {
                            sSQL += "INSERT INTO PDV.SB_KSCMPANCAVGYTMP ";
                            sSQL += sSQL + "(STD_DT,KSCM_GRP_CD,KSCM_GRP_NM,TNR_CD,REG_DTM,REGR_ID,MDFY_DTM,MDFYR_ID,TNR_VAL) ";
                            sSQL += sSQL + "VALUES (" + std_dt + "," + groupName + "," + groupCode + ",'" + tnrCode + "',SYSDATE,'thlee',SYSDATE,'thlee'," + tnrValue + ")";

                            // DB 실행
                            pgOraCmd.CommandText = sSQL;
                            pgOraCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            MessageBox.Show("DB 입력 성공!.", "금융투자협회 BIS DB 입력", MessageBoxButtons.OKCancel);
        }
        ///<summary>
        ///1. URL OPEN
        ///2. 페이지 접근
        ///3. 해당 프레임 접근
        ///</summary>
        ///<remarks>
        ///[2023-01-10 이태희] 최초 작성
        ///</remarks>
        private string openUrlAndAccessFrame()
        {
            driver = new ChromeDriver(driverService, options);

            driver.Navigate().GoToUrl("https://www.kofiabond.or.kr/index.html");

            streamWriter = File.AppendText(logPath);
            streamWriter.WriteLine(DateTime.Now + " | 브라우저 열기!");
            streamWriter.Close();

            Thread.Sleep(2000);

            driver.SwitchTo().Window(driver.WindowHandles[0]);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var mainPageframe = wait.Until(drv => drv.FindElement(By.XPath("/html/frameset/frame[2]")));

            driver.SwitchTo().Frame(mainPageframe);

            var markToMarketElement = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div/div[2]/div/ul/li[7]")); //시가평가 탭 클릭

            markToMarketElement.Click();

            Thread.Sleep(2000);

            var bondMarkToMarketStandardYTMElement = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div/div[2]/div/ul/li[7]/ul/li[1]/a/div")); //시가평가 -> 채권시가평가기준수익률 탭 클릭

            bondMarkToMarketStandardYTMElement.Click();

            Thread.Sleep(2000);

            driver.SwitchTo().DefaultContent();

            var markToMarketframe1 = wait.Until(drv => drv.FindElement(By.XPath("/html/frameset/frame[2]"))); // 페이지에 대한 Frame 접근

            driver.SwitchTo().Frame(markToMarketframe1);

            var markToMarketframe2 = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[2]/div[2]/div/div[2]/iframe"))); // 일자별 / 기간별 부분에 대한 frame 접근

            driver.SwitchTo().Frame(markToMarketframe2);

            var markToMarketframe3 = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div/div[2]/div/div/div[1]/iframe"))); // 조회 / 엑셀 다운 / 테이블에 대한 frame 접근

            driver.SwitchTo().Frame(markToMarketframe3);

            var inputDateElement = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div[2]/div/div/table/tbody/tr/td[1]/input")); //채권시가평가기준수익률 날짜

            var todayDate = inputDateElement.GetAttribute("value"); // 조회일자 가져오기

            standardTime.Value = Convert.ToDateTime(todayDate);

            streamWriter = File.AppendText(logPath);
            streamWriter.WriteLine(DateTime.Now + " | 조회일자 확인!");
            streamWriter.Close();

            return todayDate;
        }

        ///<summary>
        ///※ 오늘 날짜와 조회일자가 같으면 작업 진행 ※
        ///1. 엑셀 다운로드
        ///2. 다운로드 폴더 -> 파일 저장 폴더로 복사
        ///3. 평가사 체크 
        ///4. 조회
        ///5. 데이터 수집 
        ///</summary>
        ///<remarks>
        ///[2023-01-10 이태희] 최초 작성
        ///</remarks>
        private void crawlingData(string todayDate)
        {

            // "평가사를 선택하시기 바랍니다" 문구 확인
            if (driver.FindElement(By.XPath("/html/body/div[3]/div/div[1]")).Displayed)
            {
                // "Yes" 버튼 클릭
                var popupBondAppraiserYesElement = driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/input"));

                popupBondAppraiserYesElement.Click();
            }


            // 조회일자가 오늘 날짜와 같은 경우 -> 조회 시작 
            if (todayDate == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                streamWriter = File.AppendText(logPath);
                streamWriter.WriteLine(DateTime.Now + " | 조회일자와 오늘 날짜가 같기 때문에 데이터 자동 수집 시작!");
                streamWriter.Close();

                // Selet 드롭다운에서 미리 설정해놓은 배열값(4사)을 지정해놓는 코드
                var bondAppraiserElement = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div[4]/div/div/div/select"));
                SelectElement selectElement = new SelectElement(bondAppraiserElement);                
                selectElement.SelectByText("평가사 평균(`23.1.9~)");

                streamWriter = File.AppendText(logPath);
                streamWriter.WriteLine(DateTime.Now + " | 평가사 평균 콤보박스 세팅!");
                streamWriter.Close();

                Thread.Sleep(1000);

                //1. 체크할 평가사 평가사 명 확인 
                for (int index = 0; index < organizationList.Length; index++)
                {
                    var bondValuationCompany = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div[2]/div/div[" + (index + 2) + "]/table/tbody/tr/td[2]/label"));

                    streamWriter = File.AppendText(logPath);
                    streamWriter.WriteLine(DateTime.Now + " | 체크할 평가사 확인 : " + bondValuationCompany.Text);
                    streamWriter.Close();

                    //2. 체크할 평가사명이 맞으면 체크 진행
                    if (bondValuationCompany.Text.Contains(organizationList[index]) == true)
                        {
                            var correctBondValuationCompany = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div[2]/div/div["+(index + 2) +"]/table/tbody/tr/td[1]/input"));
                            correctBondValuationCompany.Click();

                            streamWriter = File.AppendText(logPath);
                            streamWriter.WriteLine(DateTime.Now + " | " + bondValuationCompany.Text + "클릭 진행!");
                            streamWriter.Close();

                        Thread.Sleep(500);
                        }
                        else
                        {
                            streamWriter = File.AppendText(logPath);
                            streamWriter.WriteLine(DateTime.Now + " | 체크할 평가사 순서가 변경되었습니다. 확인바람!");
                            streamWriter.Close();

                            MessageBox.Show("체크할 평가사 순서가 변경 되었습니다.", "금융투자협회 BIS", MessageBoxButtons.OKCancel);
                            continue;
                        }
                }

                //3. 조회버튼 Element 가져온 후 조회버튼 클릭
                var searchBtnElement = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div[4]/a/img"));
                searchBtnElement.Click();

                streamWriter = File.AppendText(logPath);
                streamWriter.WriteLine(DateTime.Now + " | 조회 버튼 클릭!" );
                streamWriter.Close();

                Thread.Sleep(500);

                // 테이블 Column 개수와 Row 개수 추출 (주의! ->> 값이 조회되지 않을 경우 trs.Count 값은 2)
                var tableElement = driver.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div[1]/div/table"));
                var trs = tableElement.FindElements(By.TagName("tr"));

                Thread.Sleep(500);

                //1. trs 크기로 조건문 생성
                //2. 3개보다 크면 테이블 값에서 원하는 값 추출 진행 
                if (trs.Count - 1 > 3)
                {
                    //4. 엑셀 다운로드
                    var excelDownloadIcon = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/a/img"));
                    excelDownloadIcon.Click();

                    streamWriter = File.AppendText(logPath);
                    streamWriter.WriteLine(DateTime.Now + " | 엑셀 다운로드 완료!");
                    streamWriter.Close();

                    Thread.Sleep(2000);

                    //5. 다운로드 엑셀 파일을 보관할 폴더에 복사
                    copyPath = Directory.GetCurrentDirectory() + @"\MTMV_Excel\채권시가평가기준수익률_" + todayDate + ".xls";
                    bakPath = Directory.GetCurrentDirectory() + @"\MTMV_Excel\채권시가평가기준수익률_" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "_bak.xls";

                    // 파일 존재 유무 확인
                    if (System.IO.File.Exists(copyPath) == true)
                    {
                        streamWriter = File.AppendText(logPath);
                        streamWriter.WriteLine(DateTime.Now + " | 채권시가평가기준수익률 엑셀 파일 존재함! -> 백업 진행!");
                        streamWriter.Close();

                        System.IO.File.Move(copyPath, bakPath);
                    }

                    System.IO.File.Copy(donwloadPath, copyPath,true);
                    Thread.Sleep(500);
                    System.IO.File.Delete(donwloadPath);

                    streamWriter = File.AppendText(logPath);
                    streamWriter.WriteLine(DateTime.Now + " | 다운로드 폴더에서 실행 폴더로 파일 복사!");
                    streamWriter.Close();

                    streamWriter = File.AppendText(logPath);
                    streamWriter.WriteLine(DateTime.Now + " | 데이터 자동 수집 진행 시작!");
                    streamWriter.Close();

                    for (int rowIndex = 1; rowIndex < trs.Count - 1; rowIndex++)
                    {
                        grdv_AVG_TNRVAL.AddNewRow();
                        grdv_AVG_TNRVAL.UpdateCurrentRow();
                        //dtblTable = (DataTable)grdv_AVG_TNRVAL.DataSource;
                        for (int columnIndex = 1; columnIndex < 21; columnIndex++)
                        {
                            //3. 값을 가져온다.(4번째 값은 표에는 숨겨져 있기 때문에 continue로 진행 
                            if (columnIndex == 4)
                            {
                                continue;
                            }
                            else
                            {
                                var tdElement = driver.FindElement(By.XPath("/ html/body/div[1]/div[3]/div/div[1]/div/table/tbody/tr[" + rowIndex + "]/td[" + columnIndex  + "]"));

                                //4. 그리드에 데이터 뿌려야됨. 
                                grdv_AVG_TNRVAL.SetRowCellValue(rowIndex-1, grdv_AVG_TNRVAL.Columns[columnHeaderList[columnIndex - 1]] ,tdElement.Text);
                                grdv_AVG_TNRVAL.UpdateCurrentRow();

                                //4. 크롤링 한 값을 배열 값에 대입 (추후 진행 여부에 따라서 배열이 필요 없을 수도 있음. )
                                bondMarkToMarketList[rowIndex - 1, columnIndex -1] = tdElement.Text;
                                //5. 크롤링 한 값으로 개발계 계정에 INSERT 진행
                                //InsertMarkToMarketValue(bondMarkToMarketList[rowIndex - 1, columnIndex-1]);

                                tempStr = tempStr + tdElement.Text + " | ";
                            }
                        }
                        bondMarkToMarketValueList[rowIndex - 1] = tempStr;
                        tempStr = "";
                        grdv_AVG_TNRVAL.BestFitColumns(); // 열 너비 맞추기 
                    }
                }
                else
                {
                    //2. 해당 크기보다 작으면 continue 
                    streamWriter = File.AppendText(logPath);
                    streamWriter.WriteLine(DateTime.Now + " | 채권시가평가기준수익률이 공시 되지 않았습니다! ");
                    streamWriter.Close();

                    MessageBox.Show("채권시가평가기준수익률이 공시되지 않았습니다.", "금융투자협회 BIS", MessageBoxButtons.OKCancel);
                    driver.Quit();
                    Application.Exit();
                }
                //6. 텍스트 파일 생성 
                //createTxtFile(bondMarkToMarketValueList, organizationList, index);
                //}
            }
            else
            {
                streamWriter = File.AppendText(logPath);
                streamWriter.WriteLine(DateTime.Now + " | 금일 날짜와 조회일자가 맞지 않아 작업 불가합니다!");
                streamWriter.Close();

                MessageBox.Show("채권시가평가기준수익률이 공시되지 않았습니다.", "금융투자협회 BIS", MessageBoxButtons.OKCancel);
                driver.Quit();
                Application.Exit();
            }

            MessageBox.Show("금융투자협회 BIS 채권시가평가기준수익률_" + todayDate + " 일자 데이터 자동화 수집 완료!", "금융투자협회 BIS 데이터 자동화 수집", MessageBoxButtons.OKCancel);

            streamWriter = File.AppendText(logPath);
            streamWriter.WriteLine(DateTime.Now + " | 데이터 자동 수집 진행 완료!");
            streamWriter.Close();
        }

        ///<summary>
        ///크롤링 한 데이터 TXT 파일로 작성
        ///</summary>
        ///<remarks>
        ///</remarks>
        private void createTxtFile(String[] todayMarkToMarketList, String[] organizationList, int index)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var addFilePath = @"\saveFile\" + organizationList[index] + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            for (int i = 0; i < todayMarkToMarketList.Length; i++)
            {

                if (i == 0)
                {
                    StreamWriter writer;
                    writer = File.CreateText(currentDirectory + addFilePath);
                    writer.WriteLine(todayMarkToMarketList[i]);
                    writer.Close();
                }
                else
                {
                    StreamWriter writer;
                    writer = File.AppendText(currentDirectory + addFilePath);
                    writer.WriteLine(todayMarkToMarketList[i]);
                    writer.Close();
                }
            }
        }

        ///<summary>
        ///크롤링을 통해서 가져온 값을 개발계 DB에 저장
        ///</summary>
        ///<remarks>
        ///[2023-01-06 이태희] 최초 작성 []
        ///</remarks>
        private void InsertMarkToMarketValue(string tnrVal)
        {

        }


        private void initGrid()
        {
            streamWriter = File.AppendText(logPath);
            streamWriter.WriteLine(DateTime.Now + " | 그리드 세팅 작업 시작!");
            streamWriter.Close();

            sSQL = "";
            

            // Oracle 접속정보
            pgOraConn = new OracleConnection($"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + sOracleServer + ")(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + sOracleSevice + ")));User ID=" + sOracleId + ";Password=" + sOraclePassword + ";Connection Timeout=30;");
            // Oracle 접속
            pgOraConn.Open();
            pgOraCmd = pgOraConn.CreateCommand();

            sSQL += "SELECT NULL KIND       ";
            sSQL += "     , NULL KINDNAME    ";
            sSQL += "     , NULL CREDITLEVEL ";
            sSQL += "     , NULL TNRVAL_3M   ";
            sSQL += "     , NULL TNRVAL_6M   ";
            sSQL += "     , NULL TNRVAL_9M   ";
            sSQL += "     , NULL TNRVAL_1Y   ";
            sSQL += "     , NULL TNRVAL_1Y6M ";
            sSQL += "     , NULL TNRVAL_2Y   ";
            sSQL += "     , NULL TNRVAL_2Y6M ";
            sSQL += "     , NULL TNRVAL_3Y   ";
            sSQL += "     , NULL TNRVAL_4Y   ";
            sSQL += "     , NULL TNRVAL_5Y   ";
            sSQL += "     , NULL TNRVAL_7Y   ";
            sSQL += "     , NULL  TNRVAL_10Y  ";
            sSQL += "     , NULL  TNRVAL_15Y  ";
            sSQL += "     , NULL  TNRVAL_20Y  ";
            sSQL += "     , NULL  TNRVAL_30Y  ";
            sSQL += "     , NULL  TNRVAL_50Y  ";
            sSQL += " FROM DUAL              ";
            sSQL += " WHERE 1 <> 1           ";


            pgOraCmd.CommandText = sSQL;
            pgOraCmd.ExecuteNonQuery();

            // 실행된 결과를 DataTable에 저장
            OracleDataAdapter odt = new OracleDataAdapter(pgOraCmd);
            odt.Fill(dtblTable);

            // Oracle 접속 해제
            pgOraConn.Close();

            grd_AVG_TNRVAL.DataSource = dtblTable;

            streamWriter = File.AppendText(logPath);
            streamWriter.WriteLine(DateTime.Now + " | 그리드 세팅 완료!");
            streamWriter.Close();

        }


    }
}
