from typing import Protocol


class ITarget(Protocol):
    def get_response(self, request_text: str) -> str: ...


class Adaptee:
    def get_response(self, request: str, request_length: int, must_be_true: bool) -> str:
        if not must_be_true:
            raise ValueError("must_be_true is not true")
        if (len(request) != request_length):
            raise ValueError(
                f"request_length is {request_length}, but expected {len(request)}")
        return f"Response for {request}"


class Adapter:  # (ITarget):
    def __init__(self, adaptee: Adaptee) -> None:
        self._adaptee: Adaptee = adaptee

    def get_response(self, request_text: str) -> str:
        response: str = self._adaptee.get_response(
            request_text, len(request_text), True)
        return f"This is adapted response { response}"
