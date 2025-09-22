from patterns._2_1_proxy import IRequest, RequestManager, RequestManagerProxy
from patterns._2_2_1_game_decorator import IDamageActor, Character,  DefenceBuff


def test_proxy() -> None:
    request_manager: IRequest = RequestManager("8.8.8.8")
    print(request_manager.request())
    request_manager = RequestManagerProxy(request_manager)
    print(request_manager.request())
    print(request_manager.request())
    print(request_manager.request())


def test_decorator() -> None:
    def battle(first: IDamageActor, second: IDamageActor) -> None:
        while not (first.is_dead() or second.is_dead()):
            first.hit(second)
            second.hit(first)

    human: IDamageActor = Character("Human", 300, 50)
    orc: IDamageActor = Character("Orc", 350, 75)
    battle(human, orc)

    print("Rematch")
    human = Character("Human", 300, 50)
    orc = Character("Orc", 350, 75)
    buffed_human: IDamageActor = DefenceBuff(DefenceBuff(human, 25), 50)
    battle(buffed_human, orc)
