from flask import Flask, request, jsonify
from nutrition import User, Goal, CalorieCalculator

app = Flask(__name__)

calculator = CalorieCalculator()


@app.route("/health", methods=["GET"])
def health():
    return jsonify({
        "status": "ok"
    })


@app.route("/calculate", methods=["POST"])
def calculate():
    try:
        data = request.get_json()

        user = User(
            user_id=data.get("user_id", 1),
            name=data["name"],
            age=data["age"],
            weight=data["weight"],
            height=data["height"],
            activity_level=data["activity_level"]
        )

        goal = Goal(
            goal_type=data["goal_type"],
            calorie_adjustment=data["calorie_adjustment"]
        )

        target_calories = calculator.calculate_target_calories(user, goal)

        return jsonify({
            "target_calories": target_calories
        })

    except (KeyError, ValueError) as error:
        return jsonify({
            "error": str(error)
        }), 400


if __name__ == "__main__":
    import os
    port = int(os.environ.get("PORT", 10000))
    app.run(host="0.0.0.0", port=port)