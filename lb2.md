# Лабораторна робота №2. Моделювання систем
**Проєкт:** Impulse (Командна робота)
**Студент:** Богдан (Компонент: Профіль користувача)

## 2. Функціональні вимоги
* **FR-01:** Система дозволяє вводити стартові показники: вік, зріст, вага, мета, доступний час на тренування. (Пріоритет: Високий).

---

## 3. Діаграма прецедентів (Use Case)
*Інтеграція з загальною системою планування харчування.*

```mermaid
useCaseDiagram
    actor User as "Користувач"
    
    rectangle "Система планування харчування (Модуль профілю)" {
        usecase UC01 as "Ввести персональні дані (FR-01)"
        usecase UC02 as "Обрати ціль (FR-01)"
        usecase UC_Validate as "Валідувати вхідні дані"
        
        User --> UC01
        User --> UC02
        UC01 ..> UC_Validate : <<include>>
        UC02 ..> UC_Validate : <<include>>
    }

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
