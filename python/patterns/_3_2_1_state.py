from typing import Protocol, Callable


class IPass(Protocol):
    def try_pass(self, rating: int) -> str: ...


class AutomatePass:  # (IPass)
    def try_pass(self, rating: int) -> str:
        if (rating >= 90):
            return "A"
        elif (rating >= 82):
            return "B"
        elif (rating >= 74):
            return "C"
        elif (rating >= 64):
            return "D"
        return "E"


class NormalPass:  # (IPass)
    def try_pass(self, rating: int) -> str:
        print("Take a examination ticket... ")

        return "E"


class ExclusionPass:  # (IPass)
    def try_pass(self, rating: int) -> str:
        print("Non-admission to the exam")

        return "F"


class SubjectMark:
    def __init__(self, name: str, rating: int) -> None:
        self.name: str = name
        self._rating: int = rating
        self._state: IPass = self.get_state(rating)

    @property
    def rating(self) -> int:
        return self._rating

    @rating.setter
    def rating(self, value: int) -> None:
        self._state = self.get_state(value)
        self._rating = value

    state_selectors: list[
        tuple[
            Callable[[int], bool],
            IPass
        ]
    ] = [
        (
            lambda rating: 60 <= rating <= 100,
            AutomatePass()
        ),
        (
            lambda rating: 35 <= rating < 60,
            NormalPass()
        ),
        (
            lambda rating: 0 <= rating < 35,
            ExclusionPass()
        )
    ]

    def get_state(self, rating: int) -> IPass:
        for predicate, pass_state in self.state_selectors:
            if predicate(rating):
                return pass_state
        raise ValueError
    
    def pass_exam(self)->None:
        print(f"Passing exam {self.name}")
        mark:str =   self._state.try_pass (self.rating)
        print(f"Your mark is {mark}")
