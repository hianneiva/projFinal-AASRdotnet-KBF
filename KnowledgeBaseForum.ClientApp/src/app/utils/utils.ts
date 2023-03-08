export class Utils {
    public stringIsNullOrEmpty(input: string | null | undefined) {
        return input === '' || input === null || input === undefined;
    }
}
