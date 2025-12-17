from __future__ import annotations

from pathlib import Path
from sqlalchemy import create_engine
from sqlalchemy.engine import Engine
from sqlalchemy.orm import sessionmaker, Session

# ВКАЖІТЬ шлях до БД з Проєкту 1.
# Типовий варіант: sibling-проєкти в одному workspace:
#
# workspace/
#   student-grades-mvc/instance/app.sqlite        <- Проєкт 1
#   student-grades-desktop-mvp/                   <- Проєкт 2


BASE_DIR = Path(__file__).resolve().parent.parent.parent
DB_PATH: str = f"sqlite:///{BASE_DIR / 'example.db'}" 

# BASE_DIR = Path(__file__).resolve().parents[2]  # student-grades-desktop-mvp/
# DB_PATH = (BASE_DIR.parent / "student-grades-mvc" / "instance" / "app.sqlite").resolve()

engine: Engine = create_engine(f"{DB_PATH}", future=True)
SessionFactory = sessionmaker(bind=engine, autoflush=False, autocommit=False, future=True)

def get_session() -> Session:
    return SessionFactory()
