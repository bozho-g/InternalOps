import { useModalStore } from "../stores/modalStore";
import { modalRegistry } from "../utils/modalRegistry";

export function ModalRoot() {
    const { modalType, modalProps } = useModalStore();

    if (!modalType) return null;

    const ModalComponent = modalRegistry[modalType];

    if (!ModalComponent) return null;

    return <ModalComponent {...modalProps} />;
}