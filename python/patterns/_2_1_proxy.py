from random import random
from typing import Protocol


class IRequest (Protocol):
    def request(self) -> str: ...


class RequestManager:  # (IRequest):
    def __init__(self, ip: str) -> None:
        self._ip: str = ip

    def request(self) -> str:
        return f"Response from {self._ip}"


class RequestManagerProxy:  # (IRequest):
    def __init__(self, real_request_manager: IRequest) -> None:
        self._request_manager: IRequest = real_request_manager

    def check_access(self) -> bool:
        return random() < 0.5

    def log_access(self, message: str) -> None:
        print(f"Request was handle by proxy: {message}")

    def request(self) -> str:
        if not self.check_access():
            return "Proxy response"
        response: str = self._request_manager.request()
        self.log_access(response)
        return response
