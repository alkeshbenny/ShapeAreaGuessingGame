# 🧠 Shape Area Guessing Game (Console App in C#)

A fun interactive console-based game written in C# where players try to guess the area of randomly generated geometric shapes (Circle, Rectangle, Triangle) within 30 seconds.

## 🎮 Features
- Guess the area of a shape with given dimensions.
- Get real-time feedback and score tracking.
- Timer countdown with live display.
- Input validation and error handling.
- Uses abstract classes and inheritance to structure shape logic.

## 💡 Technologies Used
- C# (.NET)
- Console Application
- Object-Oriented Programming (OOP)
- Async/Await (`Task.Delay`)
- Random shape generator

## 🔢 Scoring Rules
- ✅ Correct guess (within ±0.1 range): +10 points  
- ❌ Wrong guess or time out: -5 points  
- ⚠️ Invalid input (non-numeric): No score change

