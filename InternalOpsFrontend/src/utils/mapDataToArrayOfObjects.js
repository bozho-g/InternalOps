export function mapData(enums, data) {
    return enums.map(enumItem => ({
        name: enumItem.label,
        count: data[enumItem.label] || 0
    }));
}