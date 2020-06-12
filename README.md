# Infocar driving license terms finder

Bot that scans polish driving license system (Infocar).

Usually slots in popular places are available for booking exams not earlier than 2 weeks ahead.
If somebody cancels their exam the slot is being released in the system and then anybody is free to book it.
It sometimes happens even one day before the exam. 
It'd be nice to detect such events and be first to book such a great slot (if we need license ASAP).

This bot scans periodically API and detects if there are available terms.
It will notify with sound and print dates and times of exams for a given category and dates range

## Usage
Simply dotnet run and follow steps (video below)

[![video](https://img.youtube.com/vi/mnEHKkGDIX4/0.jpg)](https://www.youtube.com/watch?v=mnEHKkGDIX4)
