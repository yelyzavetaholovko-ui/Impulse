from flask import Flask, request, jsonify

app = Flask(__name__)

# GET-ендпоінт /health
@app.route('/health', methods=['GET'])
def health_check():
    return jsonify({"status": "ok"}), 200

# POST-ендпоінт для бізнес-логіки (Розрахунок ІМТ)
@app.route('/api/bmi', methods=['POST'])
def calculate_bmi():
    try:
        data = request.get_json()
        weight = float(data.get('weightKg'))
        height = float(data.get('heightCm'))

        if weight <= 0 or height <= 0:
            return jsonify({"error": "Вага та зріст повинні бути більшими за нуль."}), 400

        height_m = height / 100.0
        bmi = round(weight / (height_m * height_m), 2)

        return jsonify({
            "weightKg": weight,
            "heightCm": height,
            "bmi": bmi,
            "message": "Розрахунок успішний"
        }), 200
        
    except Exception as e:
        return jsonify({"error": "Неправильний формат даних"}), 400

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)