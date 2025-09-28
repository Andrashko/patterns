from patterns._2_1_proxy import IRequest, RequestManager, RequestManagerProxy
from patterns._2_2_1_game_decorator import AttackBuff, IDamageActor, Character,  DefensiveBuff
from patterns._2_2_2_pythonic_proxy_decorator import RequestManagerPythonicProxy
from patterns._2_2_3_singleton_decorator import LoggerSingletonDecorator
from patterns._2_3_2_winapi_adapter import WinApiAdapter


def test_proxy() -> None:
    request_manager: IRequest = RequestManager("8.8.8.8")
    print(request_manager.request())
    request_manager = RequestManagerProxy(request_manager)
    print(request_manager.request())
    print(request_manager.request())
    print(request_manager.request())


def test_game_decorator() -> None:
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
    buffed_human: IDamageActor = AttackBuff(DefensiveBuff(human, 25), 50)
    battle(buffed_human, orc)


def test_proxy_decorator() -> None:
    request_manager = RequestManagerPythonicProxy("8.8.8.8")
    print(request_manager.request())
    print(request_manager.request())
    print(request_manager.request())


def test_logger_singleton_decorator() -> None:
    logger1 = LoggerSingletonDecorator("log.txt")
    logger1.log("message to logger 1")
    logger2 = LoggerSingletonDecorator("other.txt")
    logger2.log("message to logger 2")
    logger1.log("other message to logger 1")
    logger1.show_log()
    logger2.show_log()


def test_winapi_adapter() -> None:
    process: str = "notepad.exe"
    adapter = WinApiAdapter()
    adapter.create_process(process)
