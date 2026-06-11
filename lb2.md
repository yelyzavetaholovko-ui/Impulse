# Лабораторна робота №2. Моделювання систем
**Проєкт:** Impulse (Командна робота)
**Студент:** Богдан (Компонент: Профіль користувача)

## 2. Функціональні вимоги
* **FR-01:** Система дозволяє вводити стартові показники: вік, зріст, вага, мета, доступний час на тренування. (Пріоритет: Високий).

---

## 3. Діаграма прецедентів (Use Case)
*Інтеграція з загальною системою планування харчування.*

```mermaid
graph LR
    User((Користувач)) --- UC01[Ввести персональні дані FR-01]
    User((Користувач)) --- UC02[Обрати ціль FR-01]
    
    UC01 -.->|include| UC_Validate[Валідувати вхідні дані]
    UC02 -.->|include| UC_Validate
    
    style User fill:#fff,stroke:#333,stroke-width:2px
    style UC01 fill:#f9f9f9,stroke:#333,stroke-width:1px
    style UC02 fill:#f9f9f9,stroke:#333,stroke-width:1px
    style UC_Validate fill:#f9f9f9,stroke:#333,stroke-width:1px

classDiagram
    class User {
        +int userId
        +String name
        -int age
        +float weight
        -float height
        +String activityLevel
        +getProfile() String
        +updateData(weight, height) void
        +enterMetrics(age, weight, height, activityLevel) bool
    }

    class Goal {
        +String type
        -float calorieAdjustment
        +applyAdjustment(baseCalories) float
    }

    User "1" -- "1" Goal : has

sequenceDiagram
    actor U as Користувач
    participant UI as Інтерфейс (Профіль)
    participant Usr as c:User
    
    U->>UI: Вводить вік, зріст, вага, активність та ціль
    UI->>Usr: enterMetrics(age, weight, height, activityLevel)
    activate Usr
    alt Перевірка успішна (значення > 0)
        Usr-->>UI: true (Дані збережено)
        UI-->>U: Повідомлення: "Дані успешно оновлено"
    else Помилка (Некоректні дані)
        Usr-->>UI: false (Validation Error)
        UI-->>U: Повідомлення: "Помилка! Перевірте введені дані"
    end
    deactivate Usr
