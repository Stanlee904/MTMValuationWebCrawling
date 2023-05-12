# MTMValuationWebCrawling

채권시가평가기준수익률 웹 크롤링

1. https://www.kofiabond.or.kr/index.html -> 금융투자협회 채권정보센터 사이트 접속

2. 평가사 선택하세요. 버튼 클릭 후 4개 기관 선택(나이스피앤아이, 한국자산평가, KIS 자산평가, 에프앤 자산평가) 후 조회 버튼 클릭

3. 엑셀 표시 클릭하여, 엑셀 다운로드 후 위에 표부분 스크래핑 후 그리드에 적재 

4. 알림 팝업 확인 후 종료 

사용언어

1. C# / 셀레니움

프로그램

1. Microsoft visual studio

2. Google Chrome

전제조건

1. visual studio 내에 셀레니움 관련 확장 프로그램을 설치 해야함. 

2. chrome 버전 또한 셀레니움 버전에 맞게 설정 되어야 하며 그러지 않으면 현재 window에 있는 chrome 버전과 충돌이 발생하여 chorme open이 안됨. 

3. window에 있는 chrome 버전을 downgrade 시켜서 버전을 맞추고 진행 하기를 권장

4. downgrade 시켰다면, chrome 버전 자동 업데이트를 막는 조치를 취하고 진행 하기를 권장함. 
