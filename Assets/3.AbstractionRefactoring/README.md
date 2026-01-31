# 3단계: 추상화를 통한 상속 리팩토링

## 📚 학습 목표
- **추상 클래스(Abstract Class)**의 개념과 사용법
- **상속(Inheritance)**을 통한 코드 재사용
- **다형성(Polymorphism)**의 이해
- **템플릿 메서드 패턴** 적용

---

## 🏗️ 구조

### 클래스 다이어그램
```
BaseBlock (추상 클래스)
├── NormalBlock (일반 블록)
├── GoldenBlock (황금 블록 - 2배 점수)
├── BigBlock (큰 블록 - 1.5배 크기)
└── FastBlock (빠른 블록 - 3배 속도)

BaseBlockSpawner (추상 클래스)
└── GameBlockSpawner (게임 로직)
```

---

## 🎯 핵심 개념

### 1. 추상 클래스 (Abstract Class)
```csharp
public abstract class BaseBlock : MonoBehaviour
{
    // 공통 기능 - 구현됨
    protected virtual void Move() { ... }
    
    // 블록별 기능 - 자식 클래스에서 구현 필수
    protected abstract void Initialize();
    public abstract int GetScore();
}
```

**특징:**
- `abstract` 키워드로 선언
- 직접 인스턴스화 불가능 (프리팹으로 못 만듦)
- 공통 기능 + 추상 메서드 조합
- 자식 클래스는 **반드시** 추상 메서드를 구현해야 함

### 2. 상속 (Inheritance)
```csharp
public class GoldenBlock : BaseBlock
{
    protected override void Initialize()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    
    public override int GetScore()
    {
        return 2;  // 황금 블록은 2점
    }
}
```

**장점:**
- ✅ 코드 중복 제거
- ✅ 유지보수 용이 (BaseBlock만 수정하면 모든 블록에 적용)
- ✅ 확장 용이 (새 블록 추가가 쉬움)

### 3. 다형성 (Polymorphism)
```csharp
// 모두 BaseBlock 타입으로 처리 가능
BaseBlock block1 = new NormalBlock();   // 1점
BaseBlock block2 = new GoldenBlock();   // 2점

block1.GetScore();  // 1 반환
block2.GetScore();  // 2 반환 (같은 메서드, 다른 결과!)
```

---

## 📁 파일 설명

### BaseBlock.cs
- **역할**: 모든 블록의 기본 기능
- **공통 기능**: Move(), Drop(), 충돌 처리
- **추상 메서드**: Initialize(), GetScore()

### NormalBlock.cs
- **특징**: 기본 블록 (1점)
- **구현**: Initialize()는 빈 구현

### GoldenBlock.cs
- **특징**: 황금 블록 (2점)
- **구현**: Initialize()에서 색상 변경

### BigBlock.cs
- **특징**: 큰 블록 (1.5배 크기)
- **구현**: Initialize()에서 크기 증가

### FastBlock.cs
- **특징**: 빠른 블록 (3배 속도)
- **구현**: Initialize()에서 속도 증가

### BaseBlockSpawner.cs
- **역할**: 블록 생성 기본 로직
- **공통 기능**: 드랍, 위치 관리, 기본 설정
- **추상 메서드**: CreateBlock()

### GameBlockSpawner.cs
- **역할**: 게임 로직 구현
- **기능**:
  - 랜덤 블록 타입 선택
  - 난이도에 따른 속도 증가
  - 확률 기반 특수 블록 출현

---

## 🔄 이전 단계와의 차이점

### 1단계 (0.Origin) - 객체지향 이전
```csharp
// enum으로 타입 관리
public enum BlockType { Normal, Golden, Big, Fast }

void ApplyBlockType()
{
    switch (blockType)
    {
        case BlockType.Golden:
            // 골든 블록 처리
            break;
        case BlockType.Big:
            // 큰 블록 처리
            break;
    }
}
```
❌ 문제점: 새 블록 추가 시 여러 곳 수정 필요

### 2단계 (1.Refactoring) - 객체지향 시도
```csharp
// 각 타입별 클래스로 분리 시도
public class GoldenBlock : MonoBehaviour { }
public class BigBlock : MonoBehaviour { }
```
❌ 문제점: 코드 중복 심함 (모든 클래스에 동일한 코드 복붙)

### 3단계 (3.AbstractionRefactoring) - 추상화 + 상속
```csharp
public abstract class BaseBlock { }  // 공통 기능
public class GoldenBlock : BaseBlock { }  // 차이점만 구현
```
✅ 해결: 공통 기능은 한 곳에, 차이점만 각 클래스에

---

## 🎮 사용 방법

### 1. 프리팹 생성
각 블록별로 프리팹 만들기:
- NormalBlock 컴포넌트 붙인 프리팹
- GoldenBlock 컴포넌트 붙인 프리팹
- BigBlock 컴포넌트 붙인 프리팹
- FastBlock 컴포넌트 붙인 프리팹

### 2. GameBlockSpawner 설정
- Inspector에서 각 프리팹 연결
- 확률 조정 가능
- 난이도 설정 조정 가능

### 3. 확장 (새 블록 추가)
```csharp
public class SuperBlock : BaseBlock
{
    protected override void Initialize()
    {
        // 나만의 초기화
        speed *= 5f;
        transform.localScale *= 2f;
    }
    
    public override int GetScore()
    {
        return 5;  // 5점!
    }
}
```

---

## 💡 교육 포인트

### 학생들에게 강조할 점:

1. **추상 클래스 vs 일반 클래스**
   - 추상 클래스는 "설계도의 설계도"
   - 공통 기능을 모아두는 곳

2. **왜 상속을 사용하는가?**
   - DRY 원칙 (Don't Repeat Yourself)
   - 한 곳만 고치면 모두 고쳐짐

3. **다형성의 힘**
   - BaseBlock 타입으로 모든 블록 처리
   - 실제 동작은 각자 다름

4. **확장성**
   - 새 블록 추가가 쉬움
   - 기존 코드 수정 최소화

---

## 🧪 실습 과제

1. **새 블록 만들기**
   - TinyBlock: 크기 0.5배
   - SlowBlock: 속도 0.3배
   - RainbowBlock: 색상이 계속 변함

2. **점수 시스템 확장**
   - ComboBlock: 연속으로 쌓으면 점수 증가
   - BonusBlock: 시간에 따라 점수 변화

3. **스포너 개선**
   - 시간에 따라 특수 블록 확률 증가
   - 연속 성공 시 황금 블록 확률 증가

---

## 📝 정리

| 단계 | 방법 | 장점 | 단점 |
|------|------|------|------|
| 0단계 | enum + switch | 간단함 | 확장 어려움 |
| 1단계 | 클래스 분리 | 타입 안전 | 코드 중복 |
| **2단계** | **추상화 + 상속** | **재사용성, 확장성** | **설계 필요** |

추상화와 상속을 통해 **유지보수가 쉽고 확장 가능한 구조**를 만들었습니다! 🎉
