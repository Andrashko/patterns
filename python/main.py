from test_patterns.creational import test_classic_fabric_method, test_classic_singleton, test_new_fabric_method, test_prototype, test_pythonic_singleton, test_metaclass_singleton, test_logger_singleton, test_auto_register_fabric_method, test_builder, test_fabric, test_auto_register_fabric


def main():
    test_classic_singleton()
    test_pythonic_singleton()
    test_metaclass_singleton()
    test_logger_singleton()
    test_classic_fabric_method()
    test_new_fabric_method()
    test_auto_register_fabric_method()
    test_fabric()
    test_auto_register_fabric()
    test_builder()
    test_prototype()
    ...


if __name__ == "__main__":
    main()
