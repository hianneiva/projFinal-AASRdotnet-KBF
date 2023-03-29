import { Tag } from "./tag";
import { Topico } from "./topico";

export class TopicoTag {
    public tagId!: string;
    public topicoId!: string;
    public topico!: Topico;
    public tag!: Tag;
}
