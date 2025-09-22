from __future__ import annotations
from patterns._2_1_proxy import IRequest, RequestManager
from typing import TypeVar, Type, Protocol
from functools import wraps
from random import random

T = TypeVar("T", bound=IRequest)


class ProxyAugmented(IRequest, Protocol):
    def check_access(self) -> bool:
        ...

    def log_access(self, message: str) -> None:
        ...


def proxy_request(cls: Type[T]) -> Type[ProxyAugmented]:
    def check_access(self: T) -> bool:
        return random() < 0.5

    def log_access(self: T, message: str) -> None:
        print(f"Request was handle by proxy: {message}")

    setattr(cls, "check_access", check_access)
    setattr(cls, "log_access", log_access)

    original_request = cls.request

    @wraps(original_request)
    def wrapped(self: ProxyAugmented) -> str:
        if not self.check_access():
            return "Proxy response"
        response: str = original_request(self)
        self.log_access(response)
        return response

    setattr(cls, "request", wrapped)
    return cls


@proxy_request
class RequestManagerPythonicProxy(RequestManager):
    ...
