from __future__ import annotations
from typing import Any, ClassVar

from patterns._1_2_1_classic_fabric_method import Document, WordApp, PDFApp


class AutoRegisterApplicationFabricMethod:  # (Application):
    _registry: dict[str, type[AutoRegisterApplicationFabricMethod]] = {}
    kind: ClassVar[str]

    def __init_subclass__(cls, **kwargs: Any) -> None:
        super().__init_subclass__(**kwargs)
        AutoRegisterApplicationFabricMethod._registry[cls.kind] = cls

    @classmethod
    def create(cls, kind: str, **kwargs: Any) -> AutoRegisterApplicationFabricMethod:
        try:
            app_cls: type[AutoRegisterApplicationFabricMethod] = cls._registry[kind]
            return app_cls(**kwargs)
        except KeyError:
            raise ValueError(f"Unknown kind: {kind}")
    #для сумісності з протоколом Application
    def create_document(self) -> Document: ...
    def open_document(self) -> None: ...


class AutoRegisterWordApp (WordApp, AutoRegisterApplicationFabricMethod):
    kind = "word"


class AutoRegisterPDFApp (PDFApp, AutoRegisterApplicationFabricMethod):
    kind = "pdf"
