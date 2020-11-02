document.addEventListener('DOMContentLoaded', (event) => {
    let lessonDescription = document.querySelector("#lessonDescription");
    let submitBtn = document.querySelector("#btn_submit");
    let antiForgeryToken = document.querySelector("[name='__RequestVerificationToken']").value;
    let week = parseInt(document.querySelector("#week").value);
    let subjectInstanceId = parseInt(document.querySelector("#subjectInstanceId").value);
    let timeFrameId = parseInt(document.querySelector("#timeFrameId").value);
    let alertDiv = document.querySelector("#alert");
    submitBtn.addEventListener("click", () => {
        let checkboxes = document.querySelectorAll(".absence-checkbox:checked");
        let absentStudentIds = [];
        for (let i of checkboxes) {
            let studentId = parseInt(i.getAttribute("student-id"));
            absentStudentIds.push(studentId);
        }
        let body = JSON.stringify({
            LessonDescription: lessonDescription.value,
            AbsentStudents: absentStudentIds,
            TimeFrameId: timeFrameId,
            Week: week,
            SubjectInstanceId: subjectInstanceId
        });
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/Teacher/LessonRecords/Index", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.setRequestHeader("RequestVerificationToken", antiForgeryToken);
        xhr.send(body);

        xhr.onload = () => {
            if (xhr.status == 200) {
                alertDiv.classList.remove("alert alert-danger");
                alertDiv.textContent = "Hodina byla úspěšně zapsána";
                alertDiv.classList.add("alert alert-success");

            } else {
                alertDiv.classList.remove("alert alert-success");
                alertDiv.textContent = "Chyba";
                alertDiv.classList.add("alert alert-danger");
            }
        }
    });
});