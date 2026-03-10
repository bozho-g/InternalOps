import { ChangePasswordModal } from "../components/modals/ChangePasswordModal";
import { NewRequestModal } from "../components/modals/NewRequestModal";
import { DeleteRequestModal } from "../components/modals/DeleteRequestModal";

export const modalRegistry = {
  "DELETE_REQUEST": DeleteRequestModal,
  "CHANGE_PASSWORD": ChangePasswordModal,
  "NEW_REQUEST": NewRequestModal
};