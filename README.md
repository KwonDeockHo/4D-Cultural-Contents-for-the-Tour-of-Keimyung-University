# 4D-Cultural-Contents-for-the-Tour-of-Keimyung-University
Visualization cultural contents using VR technology

<a href="http://kon9383.godohosting.com/content/project/project_01.php" target="_blank">홈페이지 바로가기</a>

#Unity#VR#VirtualReality#4D#Drone#VR시트#3축의자#가상현실#계명대학교#홍보플랫폼
2017년 1학기 계명대학교 캡스톤디자인을 위해 개발된 홍보플랫폼으로 기존의 홍보책자 혹은 홍보물과는 다른 입체적이고 활동적인 프로그램으로 삼성 기어 360을 드론(팬텀4프로)에 장착하여 계명대학교를 직접 촬영하고, 촬영된 영상을 Unity엔진을 이용하여 오큘러스 리프트를 통해 출력하고, VR시트를 이용하여 움직임을 제어한다. 비상업적 프로그램으로 드론과 가상현실을 결합함으로써 새로운 시장을 찾는 돌파구 역할을 기대한다.

[EQUIPMENT]
1) DRONE(Phantom4 Pro)
    학교 전경을 촬영하기위해 항공촬영이 필요하여 드론을 사용하였으며, 직접 컨트롤하였다.

2) Samsung Gear 360
    기존의 드론은 360촬영이 불가하여 Gear 360을 드론에 부착하여 360도의 영상을 촬영하였다.

3) Oculus
    학교를 자유롭게 볼 수 있게 360으로 촬영하였기 때문에 성능이 뛰어나고 가벼운 오큘러스를 사용함.

4) Inno Simulator(3-Axis)
    기존의 홍보플랫폼과의 차별점을 위해 활동적인(Active) 연출을 위해 3축 의자를 사용하였다.

[SKILL]
1) 드론 + 기어360(카메라)으로 학교의 전경을 촬영하였다. 러닝타임으로 인해 총 4구역으로 나누어서 촬영하였다.
    * 활동적인 연출을 위해 드론촬영 시 도로를 따라 상승/하강, 이동으로 드론의 움직임을 연출함.

2) Intro Scene의 경우 Curved UI를 이용하여 반원 형태의 UI로 구성하였다. 총 4가지의 구역으로 구분하였으며, 헤드 트래킹을 통해 메뉴 선택을 할 수 있다.
3) Head Tracking을 이용하여 메뉴 선택 시 일정시간(5s) 동안 트래킹 시 화면 선택을 하고, Progress Bar(원형)가 동작한며 Fade Out을 통해 화면전환.
4) 드론으로 촬영한 영상을 Unity에 원형의 구에 텍스쳐로 입힌 후 카메라 시점이 원형 구 안에서 볼 수 있게 스크립트를 제작.
5) 영상에는 학교 소개 나레이션과 음악을 편집하여 구성한다.
    * 각각의 구역별 컨셉을 지정하여 재미요소를 추가함.

6) 드론의 움직임에 따라 이노시뮬레이터(3축의자)의 움직임을 제어한다.
7) 특정구간(학교 전경을 볼 수 있는곳)에서는 영상과 움직임을 멈추고, 각 건물의 소개를 트래킹을 통해 확인할 수 있는 컨텐츠 제작.


[DRAWbACK]
<br>★ 전문적인촬영기술이 부족하여 영상의 화질이 낮다는 점.
    <br> * 기어 360의 경우 VR시점으로 전환 시 날씨에 영향 받으며, 화질이 낮다.
    <br> * 드론 촬영기술의 한계로 좀 더 부드러우며 활동적인 모션을 촬영하기 어려웠다.

<br>★ 그래픽/이미지/영상편집 기술을 구성하는데 한계가 있음..
    <br> * 전문적인 그래픽 기술이 부족하여 영상의 퀄리티가 저하.
    <br> * UI의 배경 이미지 혹은 전문적인 영상편집이 이루어 진다면 발전된 컨텐츠 개발 가능.


[DIFFERENT]
기존의 홍보 플랫폼은 책자, 영상으로만 이루어져 있으므로 예비신입생과 일반 학부모/관계자에게는 진부하고 흥미요소를 느끼지 못하는 방안이지만, 현재 프로젝트를 통해 프로그램을 만든다면 새로운 체험형 홍보플랫폼으로서, 예비 신입생/관계자 등에게 학교를 홍보하는데 차별화되고 흥미를 이끌 수 있는 효과적인 대안이다.

★ 개발언어/엔진
Unity / Oculus, Innomotion 3축 VR시트

★ 개발장비
Oculus, 3축 VR시트, 삼성 기어 360 카메라, 드론(Phantom4 Pro)

★ 개발인원
4명(역할 : 팀장)

★ 개발기간
2017.5 – 2017.6 / 2개월

★ 개발내용
<ul>
<li> 드론과 360카메라를 이용한 영상촬영</li>
<li> 촬영한 360도 영상을 이용하여 Unity와 연동(스크립트 제작)</li>
<li> Unity + VR시트 연동 및 VR시트 움직인 설계/구현</li>
<li> Curved UI를 이용한 Intro Scene</li>
<li> 시선 트래킹을 통한 UI 제어</li>
<li> 영상연출/건물소개 UI 제작</li>
</ul>
