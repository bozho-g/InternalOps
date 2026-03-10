import { useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useQueryClient } from "@tanstack/react-query";
import { useAuthStore } from "../../stores/authStore";
import { useNotificationStore } from "../../stores/notificationStore";
import { toast } from "sonner";

export function useSignalRNotifications() {
    const queryClient = useQueryClient();
    const { user, accessToken } = useAuthStore();
    const openDropdown = useNotificationStore((s) => s.open);

    useEffect(() => {
        if (!user || !accessToken) return;

        const connection = new HubConnectionBuilder()
            .withUrl("/hubs/notifications", {
                accessTokenFactory: () => accessToken
            })
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => {
                connection.on("ReceiveNotification", (notification) => {
                    queryClient.setQueryData(["notifications"], (old = []) => [
                        notification,
                        ...old
                    ]);

                    toast.info(notification.message, {
                        action: {
                            label: "View",
                            onClick: () => openDropdown(),
                        },
                    });
                });
            })
            .catch(err => console.error("SignalR connection error:", err));

        return () => {
            connection.stop();
        };
    }, [queryClient, accessToken, user, openDropdown]);
}
