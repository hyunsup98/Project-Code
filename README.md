    # 포트폴리오 코드를 정리해놓은 Repository 입니다.

**아래의 주요 기능 코드 외에도 전체 코드가 담겨져있습니다.**
**<br>스크립트의 파란 글씨를 누르시면 해당 스크립트로 이동하여 보실 수 있습니다.**

<br><br><br>

# 목차

## 1. [카오스 던전](#1-카오스-던전-1)

## 2. [Poly Hero](#2-Poly-Hero-1)

## 3. [MiniGame Rush! (예정)]()

<br><br><br>

# 1. 카오스 던전

- **프로젝트 소개**
  - 2D 횡스크롤뷰 로그라이크 게임입니다.
    
    총 3개의 스테이지로 이루어져있습니다.
  
    패시브 스킬을 정하고 여러 무기들을 활용하여 몬스터를 물리치고 마지막 보스를 잡아보세요.

<br>

## 1-1 주요 기능 코드

- **GameManager.cs**
  - [GameManager.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/System/GameManager.cs)
  - 게임의 상태 및 중요 변수 초기화를 합니다.

<br><br>

- **ObjectPool.cs**
  - [ObjectPool.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Utll/ObjectPool.cs)
  - 오브젝트의 효율적인 관리를 위해 오브젝트 풀링 기법을 사용하였습니다.
  - 오브젝트 풀링을 사용하는 객체가 여러개이므로 제네릭을 활용해 커스텀 ObjectPool 클래스를 작성했습니다.

<br><br>

- **MapManager.cs**
  - [MapManager.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Map/MapManager.cs)
  - 스테이지에 진입 시 랜덤으로 맵을 생성해주는 클래스입니다.
  - 이차원 배열을 이용해 맵 틀을 초기화하고 랜덤한 좌표부터 맵을 생성해 나가는 방식입니다.


<br><br>

- **Player.cs**
  - [Player.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Entity/Player/Player.cs)
  - 플레이어 조작, 판정 등을 관리하는 클래스입니다.
  - Spine으로 만들어진 캐릭터로 Spine API를 이용해 애니메이션의 전환과 재생을 관리했습니다. 


<br><br>

- **DialogManager.cs**
  - [DialogManager.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/System/DialogManager.cs)
  - npc의 아이디, 대사를 저장한 csv 파일을 파싱해 관리하는 클래스입니다.
  - npc의 아이디와 대조후 각 npc에게 맞는 대사를 반환합니다.


<br><br>

- **Item.cs | ItemDropTable.cs**
  - [Item.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Item/Item.cs)   [ItemDropTable.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Item/ItemDropTable.cs)
- Item.cs
  - 아이템의 구성 및 개수를 관리하는 클래스입니다.
- ItemDropTable.cs
  - 보물상자를 열거나 몬스터를 처치할 때 드랍될 아이템을 관리하는 드랍테이블 클래스입니다.
  - Scriptable Object 방식으로 모듈화를 진행해 드랍테이블의 추가 및 관리가 유연해지도록 설계했습니다.


<br><br>

- **Boss.cs**
  - [Boss.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Entity/Boss/Boss.cs)
  - Boss.cs 상속하는 클래스: [Boss_Fishron](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Entity/Boss/Boss_Fishron.cs) | [Boss_Magician](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Entity/Boss/Boss_Magician.cs) | [Boss_Slime](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Entity/Boss/Boss_Slime.cs)
  - 보스의 이동 및 공격 패턴을 관리하는 추상클래스입니다.
  - 각 스테이지 별 보스 클래스는 해당 클래스를 상속받아 패턴 메서드를 재정의해 공격 방식을 재구성 하였습니다.


<br><br>

- **Skill.cs**
  - [Skill.cs](https://github.com/hyunsup98/Project-Code/blob/main/Chaos%20Dungeon/Chaos%20Dungeon%20Scripts/Object/Skill/Skill.cs)
  - 플레이어를 포함한 엔티티들이 사용할 스킬을 관리하는 클래스입니다.
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

- **GameManager.cs**
  - [GameManager.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/System/GameManager.cs)
  - 게임에 관련해 종합적으로 관리를 담당하는 클래스입니다.
  - 게임 상태를 관리하고 퀘스트 이벤트를 이용해 퀘스트 퀘스트 상황을 처리합니다.

<br><br>

- **DataManager.cs**
  - [DataManager.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/System/DataManager.cs)
  - 유저 데이터 및 옵션 데이터를 Json 형식으로 파싱 후 base64로 인코딩하는 방식을 사용하였습니다.

<br><br>

- **Option.cs**
  - [Option.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/Option.cs)
  - 옵션을 설정하는 UI를 관리하는 클래스입니다.
  - 사운드 볼륨 조절, 해상도, 그래픽 품질을 설정할 수 있습니다.
  - 그래픽 품질은 낮음, 보통, 높음으로 설정되어 있는 파이프라인을 가져와 적용합니다.

<br><br>

- **Singleton.cs**
  - [Singleton.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/System/Singleton.cs)
  - 제네릭을 이용해 Singleton 클래스를 상속만 하면 쉽게 싱글턴 패턴을 사용할 수 있도록 하였습니다.

<br><br>

- **Inventory.cs**
  - [Inventory.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/Inventory/Inventory.cs)
  - 관련 클래스: [Slot.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/Inventory/Slot.cs) | [DragSlot](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/Inventory/DragSlot.cs)
  - 인벤토리를 관리하는 클래스입니다.
  - 아이템의 MaxCount와 추가될 개수를 비교하여 아이템을 추가할 수 있고 슬롯끼리의 아이템 바꾸기, 아이템 제거 등의 기능이 있습니다.
  - Slot과 DragSlot에서 IPointer 인터페이스를 이용한 상호작용을 통해 여러 기능을 담당하고 있습니다.

<br><br>

- **LoadingSceneController.cs**
  - [LoadingSceneController.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/LoadingSceneController.cs)
  - 씬을 넘어갈 때 로딩 화면을 띄워주기 위한 클래스입니다.
  - 비동기식으로 뒤에서 씬을 로드하며 화면에는 로딩 화면을 띄워줍니다.

<br><br>

- **NPC.cs**
  - [NPC.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Entity/NPC/NPC.cs)
  - NPC.cs를 상속하는 클래스: [NPC_BlackSmith](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Entity/NPC/NPC_BlackSmith.cs) | [NPC_ShopKeeper](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Entity/NPC/NPC_ShopKeeper.cs)
  - NPC를 관리하는 추상클래스입니다.
  - 충돌체크를 통해 대화가 가능한지 여부를 체크하고, E키를 누르면 대사 UI를 띄워 출력 및 이벤트 메서드를 실행합니다.

<br><br>

- **Collection.cs | CollectionZone.cs**
  - [Collection.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Environment/Collection.cs)    [CollectionZone.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Environment/CollectionZone.cs)
  - Collection.cs는 플레이어가 채집 가능한 자원을 관리하는 클래스입니다. 충돌 여부를 통해 플레이어가 손에 든 도구가 특정 타입일 때 채집할 수 있습니다.
  - CollectionZone.cs는 자원이 생성되는 지역으로 자원의 생성을 관리하는 클래스입니다.
  - 자원은 생성 지역 내에서 랜덤한 곳에 생성됩니다. 이때 생성될 지역에 다른 자원이 있는 경우 생성지역을 재탐색합니다. 동시에 생성 될 자원의 최대 개수를 설정할 수 있습니다.

<br><br>

- **Qeust.cs**
  - [Quest.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Quest/Quest.cs)
  - 관련 클래스: [QuestInfo](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/QuestInfo.cs) | [QuestList](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/QuestList.cs) | [QuestSlot](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/QuestSlot.cs)
  - Scriptable Object 방식을 사용한 퀘스트 클래스입니다.
  - 몬스터 처치, 자원 수집, npc 대화의 유형이 있고, 퀘스트 진행 상황은 수락 전, 수락, 완료(보상 받기 전), 완료(보상 받기 후)로 나뉩니다.

<br><br>

- **Boss_BoneDragon.cs**
  - [Boss_BoneDragon.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Entity/Boss_BoneDragon.cs)
  - 보스 씬의 보스를 제어하는 클래스입니다.
  - 상태패턴을 이용해서 보스의 동작을 제어하고 공격할 땐 내적과 외적을 이용하여 플레이어가 보스의 앞뒤, 좌우중 어디에 있는지 판별하여 각기 다른 패턴을 사용하게 구현했습니다.
  - 애니메이션 이벤트를 통해 보스 공격의 타격 시점과 사운드를 제어했습니다.

<br><br>

- **PlayerController.cs**
  - [PlayerController.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Entity/Player/PlayerController.cs)
  - IPlayerState 인터페이스를 이용해 플레이어가 손에 들고 있는 도구의 타입에 따라 각기 다른 애니메이션을 재생합니다.
  - 애니메이션 이벤트를 이용해서 콤보 어택 구현했습니다.
  - 레이캐스트를 이용해서 아이템이 에임상에 있는지 체크 후 획득 가능 ui를 띄우고 획득 가능한 상태로 만듭니다.
  - 자연스러운 애니메이션 전환을 위해서 블렌딩을 사용하거나 레이어를 두 개로 나누어 상황에 맞게 사용했습니다.

<br><br>

- **EntityUIManager.cs | WorldUI.cs**
  - [EntityUIManager.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/EntityUIManager.cs) | [WorldUI.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/WorldUI.cs)
  - WorldUI.cs 상속하는 클래스: [DamageText.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/DamageText.cs) | [HPBarUI](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/HPBarUI.cs) | [NpcUI](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/UI/NpcUI.cs)
  - 엔티티들의 정보를 확인할 수 있는 UI로 월드상 좌표에 위치할 UI들을 관리하는 클래스입니다.

<br><br>

- **BossEntry.cs**
  - [BossEntry.cs](https://github.com/hyunsup98/Project-Code/blob/main/Poly%20Hero/Poly%20Hero%20Scripts/Map/Scene_Dungeon1/BossEntry.cs)
  - 시네머신을 이용한 컷신을 제어하기 위한 클래스입니다.
  - 미리 만들어둔 타임라인을 재생하고 시그널을 이용해 메서드를 제어했습니다.

<br><br>

<div align="right">
  
[목차로](#목차)

</div>























