import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export const useAuthStore = create(
    persist(
        (set) => ({
            user: null,

            setAuth({ token, email, roles }) {
                localStorage.setItem("accessToken", token);
                set({ user: { email, roles } });
            },

            async logout() {
                localStorage.removeItem('accessToken');
                set({ user: null });
            },
        }),
        {
            name: 'auth-storage',
            partialize: (state) => ({ user: state.user }),
        }
    )
);