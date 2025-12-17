from __future__ import annotations
from datetime import date
from flask import Blueprint, render_template, request, redirect, url_for, flash
from sqlalchemy.exc import IntegrityError
from ..extensions import db
from ..models.grade import Grade
from ..models.student import Student
from ..models.subject import Subject

bp = Blueprint("grades", __name__)

ALLOWED_TYPES: set[str] = {"current", "module", "exam", "final"}

@bp.get("/")
def list_grades():
    grades = Grade.query.order_by(Grade.grade_date.desc(), Grade.id.desc()).all()
    return render_template("grades/list.html", grades=grades)

@bp.get("/new")
def new_grade_form():
    students = Student.query.order_by(Student.full_name.asc()).all()
    subjects = Subject.query.order_by(Subject.name.asc()).all()
    return render_template("grades/form.html", students=students, subjects=subjects)

@bp.post("/new")
def create_grade():
    student_id = (request.form.get("student_id") or "").strip()
    subject_id = (request.form.get("subject_id") or "").strip()
    value = (request.form.get("value") or "").strip()
    grade_type = (request.form.get("grade_type") or "").strip()
    grade_date_raw = (request.form.get("grade_date") or "").strip()  # YYYY-MM-DD

    if not (student_id.isdigit() and subject_id.isdigit() and value.isdigit()):
        flash("student/subject/value мають бути числами.", "error")
        return redirect(url_for("grades.new_grade_form"))

    v = int(value)
    if v < 0 or v > 100:
        flash("Оцінка має бути в межах 0..100.", "error")
        return redirect(url_for("grades.new_grade_form"))

    if grade_type not in ALLOWED_TYPES:
        flash("Некоректний тип оцінки.", "error")
        return redirect(url_for("grades.new_grade_form"))

    try:
        d = date.fromisoformat(grade_date_raw) if grade_date_raw else date.today()
    except ValueError:
        flash("Некоректна дата (формат YYYY-MM-DD).", "error")
        return redirect(url_for("grades.new_grade_form"))

    g = Grade(
        student_id=int(student_id),
        subject_id=int(subject_id),
        value=v,
        grade_type=grade_type,
        grade_date=d,
    )
    db.session.add(g)
    try:
        db.session.commit()
    except IntegrityError:
        db.session.rollback()
        flash("Такий запис оцінки вже існує (student+subject+type+date).", "error")
        return redirect(url_for("grades.new_grade_form"))

    flash("Оцінку додано.", "success")
    return redirect(url_for("grades.list_grades"))
