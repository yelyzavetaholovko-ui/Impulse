# test_nutrition.py
import sys
import os

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

import unittest
from nutrition import User, Goal, CalorieCalculator

class TestCalorieCalculator(unittest.TestCase):

    def setUp(self):
        self.calc = CalorieCalculator()

    # --- ТЕСТИ ДЛЯ validate_user_data ---

    def test_validate_user_valid_data(self):
        # Arrange (Підготовка)
        user = User(1, "Ivan", 25, 70.0, 175.0, "moderate")
        # Act (Дія) & Assert (Перевірка)
        # Техніка: EP (Позитивний сценарій, валідні середні значення)
        self.assertTrue(self.calc.validate_user_data(user))

    def test_validate_user_age_lower_boundary(self):
        # Arrange
        user = User(1, "Ivan", 10, 70.0, 175.0, "moderate")
        # Act & Assert
        # Техніка: BVA (Точна нижня межа віку)
        self.assertTrue(self.calc.validate_user_data(user))

    def test_validate_user_age_below_lower_boundary(self):
        # Arrange
        user = User(1, "Ivan", 9, 70.0, 175.0, "moderate")
        # Act & Assert
        # Техніка: BVA (Негативний, вихід за нижню межу віку)
        with self.assertRaises(ValueError):
            self.calc.validate_user_data(user)

    def test_validate_user_age_upper_boundary(self):
        # Arrange
        user = User(1, "Ivan", 100, 70.0, 175.0, "moderate")
        # Act & Assert
        # Техніка: BVA (Точна верхня межа віку)
        self.assertTrue(self.calc.validate_user_data(user))

    def test_validate_user_age_above_upper_boundary(self):
        # Arrange
        user = User(1, "Ivan", 101, 70.0, 175.0, "moderate")
        # Act & Assert
        # Техніка: BVA (Негативний, вихід за верхню межу віку)
        with self.assertRaises(ValueError):
            self.calc.validate_user_data(user)

    def test_validate_user_weight_lower_boundary(self):
        # Arrange
        user = User(1, "Ivan", 30, 30.0, 175.0, "moderate")
        # Act & Assert
        # Техніка: BVA (Точна нижня межа ваги)
        self.assertTrue(self.calc.validate_user_data(user))

    def test_validate_user_weight_below_boundary(self):
        # Arrange
        user = User(1, "Ivan", 30, 29.9, 175.0, "moderate")
        # Act & Assert
        # Техніка: BVA (Негативний, вихід за нижню межу ваги)
        with self.assertRaises(ValueError):
            self.calc.validate_user_data(user)


    # --- ТЕСТИ ДЛЯ calculate_bmr ---

    def test_calculate_bmr_success(self):
        # Arrange
        user = User(1, "Ivan", 25, 70.0, 175.0, "light")
        expected_bmr = 2301.41
        # Act
        result = self.calc.calculate_bmr(user)
        # Assert
        # Техніка: EP (Позитивний, коректний розрахунок коефіцієнта)
        self.assertEqual(result, expected_bmr)

    def test_calculate_bmr_invalid_activity(self):
        # Arrange
        user = User(1, "Ivan", 25, 70.0, 175.0, "super_active")
        # Act & Assert
        # Техніка: EP (Негативний, неіснуючий клас еквівалентності активності)
        with self.assertRaises(ValueError):
            self.calc.calculate_bmr(user)


    # --- ТЕСТИ ДЛЯ calculate_target_calories ---

    def test_calculate_target_calories_safe_limit_triggered(self):
        # Arrange
        user = User(1, "Ivan", 40, 35.0, 110.0, "sedentary") 
        goal = Goal("lose", -300.0) 
        # Act
        result = self.calc.calculate_target_calories(user, goal)
        # Assert
        # Техніка: EP (Позитивний, перевірка роботи циклу захисту від голодування)
        self.assertEqual(result, 1200.0)

if __name__ == '__main__':
    unittest.main()