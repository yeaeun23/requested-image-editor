# requested-image-editor

### 1. 기여도
- 개발 90%, 디자인 70%, 테스트 80%, 기획/DB설계 0%

### 2. 개발 기간
- 2021.01 ~ 2021.04 / 2022.12 ~ 2023.01 (총 6개월)

### 3. 프로그램(화상작업기) 특징
- 포토샵으로 사진을 편집하는 부서에서 사용하는 프로그램으로,\
  '편집 요청 확인 → 사진 다운로드 → 편집(포토샵) → 업로드'까지의 작업을 수행합니다.
- 포토샵에서 제공되는 '스크립트 이벤트 관리자' 기능을 활용해 파일 변화를 감지합니다.
- 지면 출력용 사진이기 때문에 'CMYK 모드' 및 'EPS 파일 확장자'를 처리합니다.
- 사진 하나 당 크기가 다른 3개의 사진 파일로 나눠 내부적으로 처리합니다.\
  (Real, Preview, Thumbnail)

<br>

## 기능 및 화면

### 0. 업데이트
- 프로그램 실행 시 서버와 버전을 비교합니다.
- 버전이 다를 경우, 서버에서 새로운 버전의 프로그램을 다운로드 합니다.

  ![image](https://user-images.githubusercontent.com/14077108/218962758-9170f4a9-5ad6-4491-b8a1-4d84fa144891.png)

<br>

### 1. 로그인
- 버전 체크 및 업데이트 성공 시 로그인 창이 뜹니다.
- 아이디 저장 여부를 선택할 수 있습니다.

  ![image](https://user-images.githubusercontent.com/14077108/218674708-53483284-e8a6-4f54-9289-b4aead657cdd.png)

- 아이디와 비밀번호를 체크합니다.
  - 아이디를 입력하지 않은 경우
  - 비밀번호를 입력하지 않은 경우
  - 비밀번호를 다르게 입력한 경우
  
  ![image](https://user-images.githubusercontent.com/14077108/218952879-d860482f-ef0a-45a2-8c4c-65c45ef7e6b2.png)
&nbsp;&nbsp;
  ![image](https://user-images.githubusercontent.com/14077108/218953029-f2354877-0f17-4496-8f2e-42d4263f0e7e.png)
&nbsp;&nbsp;
  ![image](https://user-images.githubusercontent.com/14077108/218953056-32ee02ca-5467-4ffb-8fcd-a65b7d6ee046.png)

<br>

### 2. 메인
- 로그인 성공 시 메인 창이 뜹니다.
- 편집 요청 리스트로, 좌측에서 선택하면 우측에서 크게 볼 수 있습니다.
- 이전에 사용했던 환경설정이 그대로 유지된 상태입니다.\
  (창 크기, 리스트 이미지 크기, 리스트 컬럼 너비, 스플리터 간격, 글꼴, 테마 등)
- 날짜, 출고(편집 및 전송) 상태 등으로 검색 가능하며, 검색된 결과는 페이징 처리됩니다.

  ![image](https://user-images.githubusercontent.com/14077108/218681459-d5bcd39f-0185-4cd7-8555-dca055cb38ab.png)

- 하단에서 '사진 정보'와 선택된 사진에 대한 '작업 내역'을 볼 수 있습니다.

  <img src="https://user-images.githubusercontent.com/14077108/219302857-3ea090f8-90b8-4025-ac4e-5cfe98c2f1a2.png">
  <img src="https://user-images.githubusercontent.com/14077108/219302971-3c8bc2c6-190c-4ba5-a13d-fd390dd6d8c8.png" height="220px">
  
- 파일 열기: 서버에서 로컬로 사진을 다운로드 한 후, 포토샵으로 열어줍니다.
- 파일 복사: 요청된 사진을 복사해 요청 리스트에 추가합니다.
  - 일반 복사: 마지막으로 편집된 사진 그대로 복사합니다. (파일명 뒤에 -copy)
  - 원화상 복사: 최초 요청된 사진 원본으로 복사합니다. (파일명 뒤에 -copy(o))
  
  <img src="https://user-images.githubusercontent.com/14077108/218681708-12f32b1f-69b4-411c-910c-0d119cd25f37.png" width="30%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/219291958-c441ce82-9829-46c2-8401-8d33eb77c189.png" width="60%" align="top">
  
- 파일 다운로드: 사진을 로컬로 다운로드 합니다. (3가지 크기별로 가능)
    
  <img src="https://user-images.githubusercontent.com/14077108/218681861-fe463612-0859-4e39-9b87-04b918bc3a1c.png" width="30%" align="top">
    
  <img src="https://user-images.githubusercontent.com/14077108/218682329-cc2c6763-45d3-4982-9902-a361cd5c1301.png" width="70%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/218682386-16fffeb4-9e31-45ed-aa11-3de66aef47cd.png" width="20%" align="top"> 
    
- 파일 삭제: 요청 리스트에서 없어지고, '상태: 휴지통'으로 검색하면 볼 수 있습니다.
- 전표 보기: 요청된 사진을 전표 형태로 보고, 인쇄합니다. (웹 브라우저에서 열기 가능)

  <img src="https://user-images.githubusercontent.com/14077108/218682576-655a267e-0766-4f42-b021-fe32352294a0.png" width="70%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/218682834-ff2ad39e-28cb-472e-b75b-78aa046544b7.png" width="20%" align="top">           
      
<br>

### 3. 작업창
- 요청된 사진을 열면 포토샵과 함께 작업창이 뜹니다.
- 이때 사진 크기별로 로컬 폴더에 각각 다운로드 됩니다. (매일 자동 삭제)
- 크롭(Crop) 요청일 경우, 작업창에 요청된 영역이 빨간 선으로 표시됩니다.

  <img src="https://user-images.githubusercontent.com/14077108/218951554-dbe91c7e-c902-473d-98e9-4225f2958744.png" width="50%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/218951343-0b6da414-dccb-4c90-aaae-5ae04b978b6f.png" width="40%" align="top">
  
  <img src="https://user-images.githubusercontent.com/14077108/218951604-58f69054-ed76-480c-a0f7-8a1af4ea615d.png" width="90%" align="top">

- 포토샵으로 사진을 편집하면서 저장(Ctrl+S)할 때마다, 원본과 비교할 수 있도록 작업창에 바로 반영됩니다.

  <img src="https://user-images.githubusercontent.com/14077108/218951995-fc0cc651-a46b-4dad-91af-84e1fe44ab42.png" width="50%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/218952012-4222f11b-ec86-4495-943e-c03f3fdafd7b.png" width="40%" align="top">
  
  <img src="https://user-images.githubusercontent.com/14077108/219278883-618b8a55-e9cb-4e8a-94e4-bc40d6443a3c.png" width="90%" align="top">

- 작업중 또는 출고 상태에 따라 아이콘과 상태 메시지가 달라져, 작업자들간의 충돌을 방지합니다.

  ![작업12](https://user-images.githubusercontent.com/14077108/137514007-5452a3e7-8cd2-4eda-ab3a-8087e2038eea.png)
  ![작업13](https://user-images.githubusercontent.com/14077108/137514457-67939ed9-1e08-4141-ad8e-09553c7208ab.png)

- 중간 저장: 편집된 사진을 출고하진 않고, 서버에 임시로 저장합니다. (다시 열면 이어서 편집 가능)
- 출고: 편집 완료된 사진을 서버로 최종 전송합니다.

<br>

### 4. 환경설정
- 글꼴 설정

  <img src="https://user-images.githubusercontent.com/14077108/218952371-d71500ba-681a-4889-a165-0d46d137a450.png" align="top">&nbsp;&nbsp;
  ![image](https://user-images.githubusercontent.com/14077108/218952407-4760dec7-5770-4008-9f18-56b2e13389cc.png)

- 테마 설정: 총 5개 테마가 있으며, 포토샵 인터페이스 컬러와 동일합니다.

  <img src="https://user-images.githubusercontent.com/14077108/219286231-286f97dd-f844-4192-ba83-c205dc22e003.png" width="45%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/219286321-9f5abc72-5b57-4e48-9c8d-d67513e199e9.png" width="45%" align="top">  
  
  <img src="https://user-images.githubusercontent.com/14077108/219286371-8cfd6cbf-9392-407d-8fa4-bda8bd5bfed7.png" width="45%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/219286418-3ec7452b-c989-43d4-b7cf-66d0fad93263.png" width="45%" align="top">  
  
  <img src="https://user-images.githubusercontent.com/14077108/219286466-1ab6a27d-3a49-4b36-a4f3-43af663adc4d.png" width="45%" align="top">

- 리스트 배경 설정 (White, Black, Transparent)

  ![테마6](https://user-images.githubusercontent.com/14077108/137455678-6f06cd18-3eb9-400f-a13d-13049ab48cb7.png)

- 리스트 아이콘 설정 (Rect, Round1, Round2)

  ![테마7](https://user-images.githubusercontent.com/14077108/137455696-9a96d24c-0960-49a3-aecf-2267124a9353.png)    

- 폴더 설정: 작업할 사진을 열 때 다운로드 되는 로컬 폴더로, 마지막 지정 경로가 유지됩니다.

  <img src="https://user-images.githubusercontent.com/14077108/218952548-196ad0a3-4869-4ebb-be93-3f1aff1bb976.png" width="60%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/218952615-31f1b530-1e54-4902-b147-4232bb725498.png" width="30%" align="top">

<br>

### 5. 기타
- 사진 리스트 크기 설정

  <img src="https://user-images.githubusercontent.com/14077108/218952760-f45b3d89-b102-4475-9f80-1e96bc23ebdd.png" width="45%" align="top">&nbsp;&nbsp;
  <img src="https://user-images.githubusercontent.com/14077108/218952726-52d13f62-73a8-44b0-a517-60aa1b8b8dbb.png" width="45%" align="top">

- 로그아웃 및 프로그램 재실행

  ![image](https://user-images.githubusercontent.com/14077108/218952810-7f1672e5-e160-4941-87bd-d689667f6881.png)

<br>

## 주요 기능 영상

