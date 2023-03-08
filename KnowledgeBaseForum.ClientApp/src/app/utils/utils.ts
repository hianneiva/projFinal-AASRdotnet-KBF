export class Utils {
    public stringIsNullOrEmpty(input: string | null | undefined) {
        return input === '' || input === null || input === undefined;
    }

    public arrayFromAny<Type>(input?: Type | Type[]): Type[] {
        if (input === null || input === undefined) {
            return [];
        }
        else {
            if (Array.isArray(input)) {
                return input;
            }
            else {
                return [input];
            }
        }
    }
}
