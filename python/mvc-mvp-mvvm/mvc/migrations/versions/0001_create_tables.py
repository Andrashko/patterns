from __future__ import annotations
from alembic import op
import sqlalchemy as sa

revision = "0001"
down_revision = None
branch_labels = None
depends_on = None

def upgrade() -> None:
    op.create_table(
        "students",
        sa.Column("id", sa.Integer(), primary_key=True),
        sa.Column("full_name", sa.String(length=200), nullable=False),
        sa.Column("group", sa.String(length=50), nullable=False),
        sa.Column("enrollment_year", sa.Integer(), nullable=False),
    )

    op.create_table(
        "subjects",
        sa.Column("id", sa.Integer(), primary_key=True),
        sa.Column("name", sa.String(length=200), nullable=False),
        sa.Column("credits", sa.Integer(), nullable=False, server_default="3"),
        sa.Column("semester", sa.Integer(), nullable=False, server_default="1"),
        sa.UniqueConstraint("name", name="uq_subject_name"),
    )

    op.create_table(
        "grades",
        sa.Column("id", sa.Integer(), primary_key=True),
        sa.Column("student_id", sa.Integer(), sa.ForeignKey("students.id", ondelete="CASCADE"), nullable=False),
        sa.Column("subject_id", sa.Integer(), sa.ForeignKey("subjects.id", ondelete="CASCADE"), nullable=False),
        sa.Column("value", sa.Integer(), nullable=False),
        sa.Column("grade_type", sa.String(length=30), nullable=False),
        sa.Column("grade_date", sa.Date(), nullable=False),
        sa.CheckConstraint("value >= 0 AND value <= 100", name="ck_grade_value_0_100"),
        sa.UniqueConstraint("student_id", "subject_id", "grade_type", "grade_date", name="uq_grade"),
    )

def downgrade() -> None:
    op.drop_table("grades")
    op.drop_table("subjects")
    op.drop_table("students")
