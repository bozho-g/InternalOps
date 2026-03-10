import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";

const isEmpty = (v) => v === null || v === undefined || v === '' || v === false;

export function useListFilters(filterConfig) {
    const [searchParams, setSearchParams] = useSearchParams();
    const [localValues, setLocalValues] = useState({});
    const [debouncedLocal, setDebouncedLocal] = useState({});
    const prevDebouncedRef = useRef(null);
    const timerRef = useRef(null);

    const debouncedKeys = useMemo(
        () => filterConfig.filter(f => f.debounced).map(f => f.key),
        [filterConfig]
    );

    useEffect(() => {
        timerRef.current = setTimeout(() => setDebouncedLocal(localValues), 400);
        return () => clearTimeout(timerRef.current);
    }, [localValues]);

    const getParamValue = useCallback((field) => {
        const raw = searchParams.get(field.key)?.trim();
        if (field.type === "checkbox") return raw === "true";
        return raw || field.defaultValue || '';
    }, [searchParams]);

    const inputValues = useMemo(() =>
        Object.fromEntries(
            filterConfig.map(field => [
                field.key,
                field.debounced && localValues[field.key] !== undefined
                    ? localValues[field.key]
                    : getParamValue(field)
            ])
        ),
        [filterConfig, localValues, getParamValue]
    );

    const filters = useMemo(() =>
        Object.fromEntries(
            filterConfig.map(field => [
                field.key,
                field.debounced && debouncedLocal[field.key] !== undefined
                    ? debouncedLocal[field.key]
                    : getParamValue(field)
            ])
        ),
        [filterConfig, debouncedLocal, getParamValue]
    );

    useEffect(() => {
        if (prevDebouncedRef.current === null) {
            prevDebouncedRef.current = { ...debouncedLocal };
            return;
        }

        const hasChanged = debouncedKeys.some(
            key => prevDebouncedRef.current[key] !== debouncedLocal[key]
        );
        if (!hasChanged) return;

        setSearchParams(prev => {
            const params = new URLSearchParams(prev);
            params.set('pageNumber', 1);

            debouncedKeys.forEach(key => {
                const value = debouncedLocal[key];
                const trimmed = typeof value === 'string' ? value.trim() : value;
                if (isEmpty(trimmed)) {
                    params.delete(key);
                } else {
                    params.set(key, trimmed);
                }
            });

            return params;
        }, { replace: true });

        prevDebouncedRef.current = { ...debouncedLocal };
    }, [debouncedLocal, debouncedKeys, setSearchParams]);

    const updateFilter = useCallback((key, value) => {
        if (key === "reset") {
            setSearchParams(new URLSearchParams());
            setLocalValues({});
            setDebouncedLocal({});
            clearTimeout(timerRef.current);
            prevDebouncedRef.current = {};
            return;
        }

        const field = filterConfig.find(f => f.key === key);
        if (field?.debounced) {
            setLocalValues(prev => ({ ...prev, [key]: value }));
            return;
        }

        setSearchParams(prev => {
            const params = new URLSearchParams(prev);
            const trimmed = typeof value === 'string' ? value.trim() : value;

            if (key !== "pageNumber") {
                params.set('pageNumber', 1);
            }

            if (isEmpty(trimmed)) {
                params.delete(key);
            } else {
                params.set(key, trimmed);
            }
            return params;
        });
    }, [setSearchParams, filterConfig]);

    return { filters, inputValues, updateFilter };
}