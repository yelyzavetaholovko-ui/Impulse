# nutrition.py

class User:
    def __init__(self, user_id: int, name: str, age: int, weight: float, height: float, activity_level: str):
        self.user_id = user_id
        self.name = name
        self.age = age
        self.weight = weight
        self.height = height
        self.activity_level = activity_level  # "sedentary", "light", "moderate", "active"

class Goal:
    def __init__(self, goal_type: str, calorie_adjustment: float):
        self.type = goal_type  # "lose", "maintain", "gain"
        self.calorie_adjustment = calorie_adjustment  # Наприклад: -500.0, 0.0, 500.0

class CalorieCalculator:
    
    # Метод 1: Валідація даних користувача (Умовні конструкції + Винятки)
    MIN_AGE = 10
    MAX_AGE = 100

    MIN_WEIGHT = 30.0
    MAX_WEIGHT = 250.0

    MIN_HEIGHT = 100.0
    MAX_HEIGHT = 250.0

    def validate_user_data(self, user: User) -> bool:
        if not (self.MIN_AGE <= user.age <= self.MAX_AGE):
            raise ValueError("Вік поза допустимим діапазоном.")

        if not (self.MIN_WEIGHT <= user.weight <= self.MAX_WEIGHT):
            raise ValueError("Вага поза допустимим діапазоном.")

        if not (self.MIN_HEIGHT <= user.height <= self.MAX_HEIGHT):
            raise ValueError("Зріст поза допустимим діапазоном.")

        return True

    # Метод 2: Розрахунок базового BMR з урахуванням активності (Розгалуження)
    ACTIVITY_MULTIPLIERS = {
        "sedentary": 1.2,
        "light": 1.375,
        "moderate": 1.55,
        "active": 1.725
    }


    def calculate_bmr(self, user: User) -> float:
        self.validate_user_data(user)

        base_bmr = (
                (10 * user.weight)
                + (6.25 * user.height)
                - (5 * user.age)
                + 5
            )

        multiplier = self.ACTIVITY_MULTIPLIERS.get(user.activity_level.lower())

        if multiplier is None:
            raise ValueError("Невідомий рівень активності.")

        return round(base_bmr * multiplier, 2)

    # Метод 3: Підсумковий розрахунок під мету з перевіркою на дефіцит (Умови + Цикл)
    MIN_SAFE_CALORIES = 1200.0
    CALORIE_ADJUSTMENT_STEP = 50


    def ensure_safe_calories(self, bmr: float, calorie_adjustment: float) -> float:
        target = bmr + calorie_adjustment

        while target < self.MIN_SAFE_CALORIES and calorie_adjustment < 0:
            calorie_adjustment += self.CALORIE_ADJUSTMENT_STEP
            target = bmr + calorie_adjustment

        return max(target, self.MIN_SAFE_CALORIES)


    def calculate_target_calories(self, user: User, goal: Goal) -> float:
        bmr = self.calculate_bmr(user)

        safe_target = self.ensure_safe_calories(bmr, goal.calorie_adjustment)

        return round(safe_target, 2)
