from typing import Any
from patterns._1_2_1_classic_fabric_method import Application, WordApp, PDFApp


APPLICATION_REGISTER: dict[str, type[Application]] = {
    "word": WordApp,
    "pdf": PDFApp
}


def application_fabric_method(kind: str, **kwargs: Any) -> Application:
    try:
        cls = APPLICATION_REGISTER[kind]
    except KeyError:
        raise ValueError(f"Невідомий тип документу {kind}")
    return cls(**kwargs)
