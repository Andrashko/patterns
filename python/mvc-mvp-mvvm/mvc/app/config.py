from __future__ import annotations
from dataclasses import dataclass
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent

@dataclass(frozen=True)
class Config:
    SECRET_KEY: str = "dev-secret-key"
    SQLALCHEMY_DATABASE_URI: str = f"sqlite:///{BASE_DIR / 'example.db'}"
    SQLALCHEMY_TRACK_MODIFICATIONS: bool = False
