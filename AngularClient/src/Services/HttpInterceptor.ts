import { HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { environment } from "../environments/environment.development";


export function BaseAddressInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {

  const baseUrl: string = environment.BASE_URL;

  const newReq = req.clone({
    url: baseUrl + req.url
  });

  return next(newReq);
}
