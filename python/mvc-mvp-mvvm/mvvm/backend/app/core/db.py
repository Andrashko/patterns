from __future__ import annotations
from sqlalchemy import create_engine
from sqlalchemy.engine import Engine
from sqlalchemy.orm import sessionmaker, Session
from .config import settings

engine: Engine = create_engine(settings.database_url, future=True)
SessionFactory = sessionmaker(bind=engine, autoflush=False, autocommit=False, future=True)

def get_session() -> Session:
    db = SessionFactory()
    try:
        yield db
    finally:
        db.close()
