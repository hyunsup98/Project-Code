    # 포트폴리오 코드를 정리해놓은 Repository 입니다.

**아래의 주요 기능 코드 외에도 전체 코드가 담겨져있습니다.**

<br><br><br>

# 목차

## 1. [Chaos Dungeon](#카오스 던전)

## 2. [Poly Hero](#2.Poly-Hero)

<br><br><br>

# 1. 카오스 던전

- **프로젝트 소개**
  - 2D 횡스크롤뷰 로그라이크 게임입니다.
    
    총 3개의 스테이지로 이루어져있습니다.
  
    패시브 스킬을 정하고 여러 무기들을 활용하여 몬스터를 물리치고 마지막 보스를 잡아보세요.

<br>

## 1-1 주요 기능 코드

- **ObjectPool.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Util/...*
  - 오브젝트의 효율적인 관리를 위해 오브젝트 풀링 기법을 사용하였습니다.
  - 오브젝트 풀링을 사용하는 객체가 여러개이므로 제네릭을 활용해 커스텀 ObjectPool 스크립트를 작성했습니다.

<br><br>

- **MapManager.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Map/...*
  - 스테이지에 진입 시 랜덤으로 맵을 생성해주는 스크립트입니다.
  - 이차원 배열을 이용해 맵 틀을 초기화하고 랜덤한 좌표부터 맵을 생성해 나가는 방식입니다.


<br><br>

- **Player.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Entity/...*
  - 플레이어 조작, 판정 등을 관리하는 스크립트입니다.
  - Spine으로 만들어진 캐릭터로 Spine API를 이용해 애니메이션의 전환과 재생을 관리했습니다. 


<br><br>

- **DialogManager.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/System/...*
  - npc의 아이디, 대사를 저장한 csv 파일을 파싱해 관리하는 스크립트입니다.
  - npc의 아이디와 대조후 각 npc에게 맞는 대사를 반환합니다.


<br><br>

- **Item.cs | ItemDropTable.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Item/...*
- Item.cs
  - 아이템의 구성 및 개수를 관리하는 스크립트입니다.
- ItemDropTable.cs
  - 보물상자를 열거나 몬스터를 처치할 때 드랍될 아이템을 관리하는 드랍테이블 스크립트입니다.
  - Scriptable Object 방식으로 모듈화를 진행해 드랍테이블의 추가 및 관리가 유연해지도록 설계했습니다.


<br><br>

- **Boss.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Entity/Boss/...*
  - 보스의 이동 및 공격 패턴을 관리하는 추상클래스입니다.
  - 각 스테이지 별 보스 클래스는 해당 클래스를 상속받아 패턴 메서드를 재정의해 공격 방식을 재구성 하였습니다.


<br><br>

- **Skill.cs**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Object/Skill/...*
  - 플레이어를 포함한 엔티티들이 사용할 스킬을 관리하는 스크립트입니다.
  - 스킬의 시작지점, 이동 속도, 회전, 대미지 등을 설정할 수 있으며 주변환경에 막히는지, 차징 스킬인지 등의 여부를 체크해 다양한 형식의 스킬을 간단하게 추가할 수 있습니다.


<br><br>

<div align="right">
  
[목차로](#목차)

</div>

<br><br><br><br><br>

# 2. Poly Hero

- **프로젝트 소개**
  - 로우폴리 스타일의 3D RPG 게임입니다.

    자원을 수집하고 아이템을 제작해 몬스터를 잡으세요. 무기를 강화하고 레벨업으로 성장해 보스를 잡아보세요!
    
<br>

## 2-1 주요 기능 코드

- **EnemyAI.cs**
  - 경로: *Poly Hero/Poly Hero Scripts/System/Pattern/Behavior Tree/...*
  - 몬스터의 AI와 동작을 관리하는 스크립트입니다.
  - 행동트리를 사용해 몬스터의 행동을 관리합니다.
- 행동트리 관련 스크립트: 같은 경로의 BehaviorTreeRunner.cs, INode.cs, ActionNode.cs, SelectorNode.cs, SequenceNode.cs

<br><br>

<div align="right">
  
[목차로](#목차)

</div>


























