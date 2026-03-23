import { Injectable } from '@angular/core';
import { ErrorHandlingService, AppError } from './error-handling.service';

declare var bootstrap: any;

type ToastType = 'success' | 'error' | 'warning' | 'info';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly containerId = 'app-toast-container';
  private readonly defaultDurationMs = 5000;

  constructor(errorHandling: ErrorHandlingService) {
    console.log('NotificationService constructor called');
    errorHandling.errors$.subscribe(error => {
      console.log('Error received in NotificationService:', error);
      this.handleAppError(error);
    });
  }

  private handleAppError(error: AppError): void {
    console.log('Handling app error:', error);
    const type: ToastType = error.severity === 'error' ? 'error' : 'success';
    this.showToast(error.message, type, error.code);
  }

  private showToast(
    message: string,
    type: ToastType,
    title?: string,
    durationMs: number = this.defaultDurationMs
  ): void {
    console.log(`Showing toast: [${type.toUpperCase()}] ${title ? title + ': ' : ''}${message}`);
    const container = this.getOrCreateContainer();
    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-bg-${this.getBootstrapColor(type)} border-0 shadow-sm`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    toast.style.minWidth = '280px';
    toast.style.marginBottom = '0.5rem';
    toast.style.pointerEvents = 'auto';

    toast.innerHTML = `
      <div class="d-flex">
        <div class="toast-body">
          ${title ? `<strong>${this.escapeHtml(title)}</strong><br/>` : ''}
          ${this.escapeHtml(message)}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto" aria-label="Close"></button>
      </div>
    `;

    const closeBtn = toast.querySelector('button');
    closeBtn?.addEventListener('click', () => this.removeToast(toast));

    container.appendChild(toast);

    // Initialize Bootstrap Toast
    if (typeof bootstrap !== 'undefined' && bootstrap.Toast) {
      const bsToast = new bootstrap.Toast(toast, { autohide: true, delay: durationMs });
      bsToast.show();

      // Remove after hide
      toast.addEventListener('hidden.bs.toast', () => {
        toast.remove();
      });
    } else {
      console.warn('Bootstrap Toast not available, falling back to manual show');
      requestAnimationFrame(() => toast.classList.add('show'));
      setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 200);
      }, durationMs);
    }
  }

  private getBootstrapColor(type: ToastType): string {
    switch (type) {
      case 'error':
        return 'danger';
      case 'success':
        return 'success';
      case 'warning':
        return 'warning';
      case 'info':
        return 'info';
      default:
        return 'secondary';
    }
  }

  private getOrCreateContainer(): HTMLElement {
    let container = document.getElementById(this.containerId);
    if (!container) {
      console.error('Toast container not found!');
      // Fallback: create it
      container = document.createElement('div');
      container.id = this.containerId;
      container.style.position = 'fixed';
      container.style.top = '1rem';
      container.style.right = '1rem';
      container.style.zIndex = '1100';
      container.style.display = 'flex';
      container.style.flexDirection = 'column';
      container.style.gap = '0.5rem';
      container.style.pointerEvents = 'none';
      document.body.appendChild(container);
    }
    return container;
  }

  private removeToast(toast: HTMLElement): void {
    toast.classList.remove('show');
    toast.style.opacity = '0';
    window.setTimeout(() => {
      toast.remove();
    }, 200);
  }

  private escapeHtml(value: string): string {
    const div = document.createElement('div');
    div.textContent = value;
    return div.innerHTML;
  }
}
