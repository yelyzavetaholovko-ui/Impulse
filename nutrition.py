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
    def validate_user_data(self, user: User) -> bool:
        if not (10 <= user.age <= 100):
            raise ValueError("Вік повинен бути в діапазоні від 10 до 100 років.")
        if not (30.0 <= user.weight <= 250.0):
            raise ValueError("Вага повинна бути в діапазоні від 30 до 250 кг.")
        if not (100.0 <= user.height <= 250.0):
            raise ValueError("Зріст повинен бути в діапазоні від 100 до 250 см.")
        return True

    # Метод 2: Розрахунок базового BMR з урахуванням активності (Розгалуження)
    def calculate_bmr(self, user: User) -> float:
        self.validate_user_data(user)
        
        # Спрощена формула Міффліна-Сан Жеора для чоловіків (в середньому)
        base_bmr = (10 * user.weight) + (6.25 * user.height) - (5 * user.age) + 5
        
        # Визначення коефіцієнта активності
        activity_multipliers = {
            "sedentary": 1.2,
            "light": 1.375,
            "moderate": 1.55,
            "active": 1.725
        }
        
        multiplier = activity_multipliers.get(user.activity_level.lower())
        if not multiplier:
            raise ValueError("Невідомий рівень активності.")
            
        return round(base_bmr * multiplier, 2)

    # Метод 3: Підсумковий розрахунок під мету з перевіркою на дефіцит (Умови + Цикл)
    def calculate_target_calories(self, user: User, goal: Goal) -> float:
        bmr = self.calculate_bmr(user)
        target = bmr + goal.calorie_adjustment
        
        # Нетривіальна логіка: безпека (калорії не можуть бути екстремально низькими)
        # Якщо дефіцит занадто великий, циклічно зменшуємо крок коригування
        min_safe_calories = 1200.0
        
        if target < min_safe_calories:
            # Імітуємо адаптивний цикл підбору безпечної норми
            while target < min_safe_calories and goal.calorie_adjustment < 0:
                goal.calorie_adjustment += 50  # Пом'якшуємо дефіцит
                target = bmr + goal.calorie_adjustment
            
            if target < min_safe_calories:
                return min_safe_calories
                
        return round(target, 2)