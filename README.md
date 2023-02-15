# requested-image-editor (화상작업기)

### 1. 기여도
- 개발 90%, 디자인 70%, 테스트 80%, 기획/DB설계 0%

### 2. 개발 기간
- 2021.01 ~ 2021.04 / 2022.12 ~ 2023.01 (총 6개월)

### 3. 프로그램 특징
- 포토샵으로 사진을 편집하는 부서에서 사용하는 프로그램으로,\
  '사진 편집 요청 확인 → 사진 편집 → 편집된 사진 전송'까지의 작업을 수행합니다.
- 포토샵에서 제공되는 '스크립트 이벤트 관리자' 기능을 활용합니다.
- 지면 출력용 사진이기 때문에 'CMYK 모드' 및 'EPS 파일 확장자'를 처리합니다.

## 기능 및 화면

### 0. 업데이트
- 프로그램 실행 시 서버와 버전을 비교합니다.
- 버전이 다를 경우, 서버에서 새로운 버전의 프로그램을 다운로드 합니다.

![image](https://user-images.githubusercontent.com/14077108/218962758-9170f4a9-5ad6-4491-b8a1-4d84fa144891.png)

### 1. 로그인
- 버전 체크 및 업데이트 성공 시 로그인 창이 뜹니다.
- 아이디 저장 여부를 체크할 수 있습니다.
- 아이디와 비밀번호를 체크합니다.
  - 아이디를 입력하지 않은 경우
  - 비밀번호를 입력하지 않은 경우
  - 비밀번호를 다르게 입력한 경우
  
![image](https://user-images.githubusercontent.com/14077108/218674708-53483284-e8a6-4f54-9289-b4aead657cdd.png)

![image](https://user-images.githubusercontent.com/14077108/218952879-d860482f-ef0a-45a2-8c4c-65c45ef7e6b2.png)

![image](https://user-images.githubusercontent.com/14077108/218953029-f2354877-0f17-4496-8f2e-42d4263f0e7e.png)

![image](https://user-images.githubusercontent.com/14077108/218953056-32ee02ca-5467-4ffb-8fcd-a65b7d6ee046.png)

- - -

### 2. 메인
- 로그인 성공 시 메인 창이 뜹니다.
- 이전에 사용했던 환경설정이 그대로 유지된 상태입니다.
  (창 크기, 리스트 이미지 크기, 리스트 컬럼 너비, 스플리터 간격, 글꼴, 테마 등)

![image](https://user-images.githubusercontent.com/14077108/137449088-5ba2342a-30d6-491b-b642-5b77a0cdb93f.png)
![image](https://user-images.githubusercontent.com/14077108/218681459-d5bcd39f-0185-4cd7-8555-dca055cb38ab.png)

#### 2-1. 좌측 뷰
- 편집 요청된 이미지 리스트
- 우클릭 메뉴

    ![우클릭2](https://user-images.githubusercontent.com/14077108/137505072-b92a1596-6ab2-4021-8d77-c03134012c01.png)
    ![image](https://user-images.githubusercontent.com/14077108/218681708-12f32b1f-69b4-411c-910c-0d119cd25f37.png)
    ![image](https://user-images.githubusercontent.com/14077108/218681861-fe463612-0859-4e39-9b87-04b918bc3a1c.png)

#### 2-2. 우측 뷰
- 좌측에서 선택된 이미지의 전표 상세 (요청사항)
- 하단 메뉴

    ![전표보기3](https://user-images.githubusercontent.com/14077108/137486000-e7cb541e-3511-40d1-a2e7-35abfe2bf6e1.png)
    ![image](https://user-images.githubusercontent.com/14077108/218682101-9ae24913-9017-42cd-9361-98f3e3e1254d.png)

    + 파일 저장 : 해상도별로 서버 → 로컬 다운로드

    ![파일저장2](https://user-images.githubusercontent.com/14077108/137497969-f5bcf803-9621-4812-b61c-037adeb5df77.png)
    ![image](https://user-images.githubusercontent.com/14077108/218682329-cc2c6763-45d3-4982-9902-a361cd5c1301.png)
    ![image](https://user-images.githubusercontent.com/14077108/218682386-16fffeb4-9e31-45ed-aa11-3de66aef47cd.png)

    + 전표 보기 : 웹뷰, 브라우저에서 열기

    ![전표보기2](https://user-images.githubusercontent.com/14077108/137481832-618219aa-6b81-41ed-8b30-7038da035030.png)
    <img src="https://user-images.githubusercontent.com/14077108/218682576-655a267e-0766-4f42-b021-fe32352294a0.png" width="45%" align="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src = "https://user-images.githubusercontent.com/14077108/218682735-be1f9b4d-1d8a-4cc9-a468-2c1b9190f227.png" width="45%" align="top">

    + 전표 인쇄

    ![image](https://user-images.githubusercontent.com/14077108/137460896-f0b2a993-2e0a-45e6-b2f4-64c97ab81e72.png)
    ![image](https://user-images.githubusercontent.com/14077108/218682834-ff2ad39e-28cb-472e-b75b-78aa046544b7.png)

- - -

### 3. 작업창
- 우클릭 메뉴에서 파일 열기 or 더블 클릭 → 포토샵 실행 및 파일 로드 → 작업창 실행

![작업10](https://user-images.githubusercontent.com/14077108/137513121-0b585ed7-69c1-4920-b300-1578a37ef2d4.png)


![image](https://user-images.githubusercontent.com/14077108/218951554-dbe91c7e-c902-473d-98e9-4225f2958744.png)

![image](https://user-images.githubusercontent.com/14077108/218951343-0b6da414-dccb-4c90-aaae-5ae04b978b6f.png)

![image](https://user-images.githubusercontent.com/14077108/218951604-58f69054-ed76-480c-a0f7-8a1af4ea615d.png)



- 편집 후 저장 → 출고

![작업11](https://user-images.githubusercontent.com/14077108/137513155-e7765ba1-49ac-44ea-98b2-7d3edc6b0327.png)

![image](https://user-images.githubusercontent.com/14077108/218951995-fc0cc651-a46b-4dad-91af-84e1fe44ab42.png)

![image](https://user-images.githubusercontent.com/14077108/218952012-4222f11b-ec86-4495-943e-c03f3fdafd7b.png)

![image](https://user-images.githubusercontent.com/14077108/218952023-5e808c79-896a-4359-83d1-ec3a0f6a20c6.png)


- 상태 변화

![작업12](https://user-images.githubusercontent.com/14077108/137514007-5452a3e7-8cd2-4eda-ab3a-8087e2038eea.png)

![작업13](https://user-images.githubusercontent.com/14077108/137514457-67939ed9-1e08-4141-ad8e-09553c7208ab.png)

![작업](https://user-images.githubusercontent.com/14077108/137519778-226dca2a-46d5-48b4-abdc-703fc1d047bb.png)

- - -

### 4. 환경설정

![환경설정](https://user-images.githubusercontent.com/14077108/137449977-a1b5af94-bf31-4797-ae61-d90c23a2c95d.png)
![image](https://user-images.githubusercontent.com/14077108/218952371-d71500ba-681a-4889-a165-0d46d137a450.png)


#### 4-1. 글꼴 설정

![image](https://user-images.githubusercontent.com/14077108/137455285-ced1a374-711b-4f4e-939c-55b6fb254955.png)
![image](https://user-images.githubusercontent.com/14077108/218952407-4760dec7-5770-4008-9f18-56b2e13389cc.png)

#### 4-2. 배경 설정

![테마6](https://user-images.githubusercontent.com/14077108/137455678-6f06cd18-3eb9-400f-a13d-13049ab48cb7.png)

#### 4-3. 아이콘 설정

![테마7](https://user-images.githubusercontent.com/14077108/137455696-9a96d24c-0960-49a3-aecf-2267124a9353.png)

- 아이콘 종류 : 전표(전표 있음) / 작업중, 출고(편집 완료), 미출고(편집 전)
    
![아이콘가이드](https://user-images.githubusercontent.com/14077108/137486990-caf6d5e5-8a77-4cf8-b703-758d2749d19b.jpg)

#### 4-4. 폴더 설정

![폴더설정2](https://user-images.githubusercontent.com/14077108/137458530-4afe3e8f-83d5-4334-9897-1451bee9950b.png)
![image](https://user-images.githubusercontent.com/14077108/218952548-196ad0a3-4869-4ebb-be93-3f1aff1bb976.png)
![image](https://user-images.githubusercontent.com/14077108/218952615-31f1b530-1e54-4902-b147-4232bb725498.png)


- - -

### 5. 기타

![기타1](https://user-images.githubusercontent.com/14077108/137480331-a6a0f902-6275-4ad1-9eac-005501935be0.png)

#### 5-1. 이미지 크기 설정

![크기설정3](https://user-images.githubusercontent.com/14077108/137459801-85afa6b5-619a-4f6b-b4cb-cec8b9c85d68.png)
![image](https://user-images.githubusercontent.com/14077108/218952760-f45b3d89-b102-4475-9f80-1e96bc23ebdd.png)

![image](https://user-images.githubusercontent.com/14077108/218952726-52d13f62-73a8-44b0-a517-60aa1b8b8dbb.png)


#### 5-2. 검색 초기화

- 이미지 리스트 컨트롤 값 초기화 (날짜, 상태 등)

#### 5-3. 새로고침
- 메인화면 갱신 (이미지 리스트), 검색 컨트롤 값은 유지
- 단축키 : F5

#### 5-4. 로그아웃

![image](https://user-images.githubusercontent.com/14077108/137480642-49a911f8-32d7-42ac-b612-1cee7e2ee1ac.png)
![image](https://user-images.githubusercontent.com/14077108/218952810-7f1672e5-e160-4941-87bd-d689667f6881.png)


