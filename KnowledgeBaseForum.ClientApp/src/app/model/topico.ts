import { BaseEntity } from "./base-entity";
import { Comentario } from "./comentario";

export class Topico extends BaseEntity {
    public titulo!: string;
    public conteudo!: string;
    public tipoAcesso!: number;
    public status!: boolean;
    public usuarioId!: string;
    public comentarios?: Comentario[];
    // TopicoTag (Relacional)
}
